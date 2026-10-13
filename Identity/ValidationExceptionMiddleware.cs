using System.Text.Json;
using FluentValidation;

public sealed class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ValidationExceptionMiddleware> _logger;

    public ValidationExceptionMiddleware(
        RequestDelegate next,
        ILogger<ValidationExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning(
                    "Cannot write validation error response because the response has already started.");

                throw;
            }

            var errors = ex.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(error => error.ErrorMessage)
                        .Distinct()
                        .ToArray());

            context.Response.Clear();
            context.Response.StatusCode =
                StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = StatusCodes.Status400BadRequest,
                title = "Validation failed",
                errors
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response),
                context.RequestAborted);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception after response started.");

                throw;
            }

            _logger.LogError(ex, "Unhandled exception");

            context.Response.Clear();
            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = StatusCodes.Status500InternalServerError,
                title = "Internal server error",
                message = "Внутренняя ошибка сервера"
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response),
                context.RequestAborted);
        }
    }
}

public static class ValidationExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseValidationExceptionHandling(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<ValidationExceptionMiddleware>();
    }
}