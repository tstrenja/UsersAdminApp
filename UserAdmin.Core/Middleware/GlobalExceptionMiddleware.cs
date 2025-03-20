using System.Net;
using System.Text.Json; 
using Microsoft.AspNetCore.Http; 
using Microsoft.Extensions.Logging;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    { 
        var statusCode = exception switch
        {
            KeyNotFoundException => (int)HttpStatusCode.NotFound,  // 404 - Resource not found
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401 - Unauthorized
            ArgumentException => (int)HttpStatusCode.BadRequest,  // 400 - Bad request
            _ => (int)HttpStatusCode.InternalServerError // 500 - Internal Server Error
        };

        var problemDetails = new 
        {
            Status = statusCode,
            Title = "An error occurred",
            Detail = exception.Message, 
            Instance = context.Request.Path
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var jsonResponse = JsonSerializer.Serialize(problemDetails);
        return context.Response.WriteAsync(jsonResponse);
    }
}
