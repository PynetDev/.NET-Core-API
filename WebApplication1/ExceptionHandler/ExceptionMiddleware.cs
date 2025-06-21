using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace WebApplication1.ExceptionHandler
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Process the request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            // Map exceptions to status codes, including 406
            context.Response.StatusCode = exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                NotAcceptableException => (int)HttpStatusCode.NotAcceptable, // Added 406 support
                _ => (int)HttpStatusCode.InternalServerError
            };

            var problem = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = GetTitleForStatusCode(context.Response.StatusCode),
                Detail = _env.IsDevelopment() ? exception.Message : "An unexpected error occurred.",
                Instance = context.Request.Path
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            return context.Response.WriteAsync(JsonSerializer.Serialize(problem, options));
        }

        private string GetTitleForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Not Found",
                406 => "Not Acceptable",    // Added 406 title
                500 => "Internal Server Error",
                _ => "Error"
            };
        }
    }
}