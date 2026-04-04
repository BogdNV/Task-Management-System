using FluentValidation;
using TaskManager.Application.Exceptions;

namespace TaskManager.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (status, title, detail) = exception switch
        {
            ValidationException ex => (400, "Validation Error", ex.Message),
            NotFoundException ex => (404, "Not Found", ex.Message),
            ForbiddenException ex => (403, "Forbidden", ex.Message),
            _ => (500, "Server Error", "Произошла непредвиденная ошибка")
        };

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";

        var respone = new
        {
            status,
            title,
            detail,
            errors = exception is ValidationException vex
                ? vex.Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,

                        g => g.Select(e => e.ErrorMessage).ToArray())
                : null
        };

        return context.Response.WriteAsJsonAsync(respone);
    }
}
