using System.Text.Json;
using FluentValidation;

namespace Catalog.Service.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning("Validation failed: {Errors}", ex.Errors);

            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { errors }));
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Resource not found.");

            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Invalid operation.");

            context.Response.StatusCode = 409;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "An unexpected error occurred." }));
        }
    }
}
