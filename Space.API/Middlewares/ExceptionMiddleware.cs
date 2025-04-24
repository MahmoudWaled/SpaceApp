using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Space.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized && !context.Response.HasStarted)
                {
                    _logger.LogWarning("Unauthorized access attempt detected.");
                    await HandleUnauthorizedAsync(context);
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden && !context.Response.HasStarted)
                {
                    _logger.LogWarning("Forbidden access attempt detected.");
                    await HandleForbiddenAsync(context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new
            {
                statusCode,
                message = exception.Message,
                errorType = exception.GetType().Name
            };

            context.Response.StatusCode = statusCode;
            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }

        private Task HandleUnauthorizedAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var response = new
            {
                statusCode = StatusCodes.Status401Unauthorized,
                message = "Unauthorized: Invalid or missing token. Please authenticate and try again.",
                errorType = "UnauthorizedAccess"
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleForbiddenAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            var response = new
            {
                statusCode = StatusCodes.Status403Forbidden,
                message = "Forbidden: You do not have permission to access this resource.",
                errorType = "ForbiddenAccess"
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}