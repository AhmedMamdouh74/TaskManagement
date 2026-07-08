using TaskManagement.API.Responses;
using TaskManagement.Application.Common.Exceptions;

namespace TaskManagement.API.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            BadRequestException => StatusCodes.Status400BadRequest,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            ConflictException => StatusCodes.Status409Conflict,
            ServerInternalException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        switch (exception)
        {
            case NotFoundException:
            case BadRequestException:
            case UnauthorizedException:
            case ConflictException:
                _logger.LogWarning(exception, exception.Message);
                break;

            default:
                _logger.LogError(exception, "An unexpected error occurred.");
                break;
        }

        var response = new ErrorResponse
        {
            Success = false,
            StatusCode = statusCode,
            Message = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred."
                : exception.Message
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(response);
    }
}