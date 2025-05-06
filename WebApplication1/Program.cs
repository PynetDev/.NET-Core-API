using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication1.Data;
using WebApplication1.Logging;

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
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
