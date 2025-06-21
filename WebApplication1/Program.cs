using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using WebApplication1.AutoMapperConfigurations;
using WebApplication1.Configurations;
using WebApplication1.Data;
using WebApplication1.ExceptionHandler;
using WebApplication1.Logging;
using WebApplication1.Repositories;
using WebApplication1.Services;


var builder = WebApplication.CreateBuilder(args);

# region Serilog Configuration
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Information()  // Starts logging from information level                        
//    .WriteTo.File("Log/log.txt", rollingInterval:RollingInterval.Day) // File Provider - logging into files
//                                                                         //RollingInterval.Minute - this will generate a new file for every minute.
//                                                                         //you can change interval to hour or daya as well
//    .CreateLogger();

//builder.Host.UseSerilog(); // Only serilog will work
//builder.Logging.AddSerilog(); // In Built and serilog both will work
#endregion

// Add services to the container.

//Content negotiation is registered with JSON and XML data if any other type the api will not show any result
builder.Services.AddControllers(options=>options.ReturnHttpNotAcceptable=true).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Add JWT Bearer Auth to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR...\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//Registering Dependency Injection
//LogToDB

//builder.Services.AddScoped<IMyLogger, LogToDB>();
//builder.Services.AddTransient<IMyLogger, LogToDB>();
//builder.Services.AddSingleton<IMyLogger, LogToDB>();

//LogToFile

//builder.Services.AddScoped<IMyLogger, LogToFile>();
//builder.Services.AddTransient<IMyLogger, LogToFile>();
//builder.Services.AddSingleton<IMyLogger, LogToFile>();

//LogToServerMemory

//builder.Services.AddScoped<IMyLogger, LogToServerMemory>();
//builder.Services.AddTransient<IMyLogger, LogToServerMemory>();
//builder.Services.AddSingleton<IMyLogger, LogToServerMemory>();

#region 
builder.Services.AddDbContext<CollegeDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeDBConnectionString")));
#endregion

//Mapper Service Register
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();

//In Memory Caching
builder.Services.AddMemoryCache();

//Jwt Authentication 
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero, // Optional: reduces token expiration buffer
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();



var app = builder.Build();

//Global Exception Handling Registartion
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
