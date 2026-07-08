using System.Text.Json;
using TaskManagement.API.Responses;
using TaskManagement.Application.Common.Exceptions;

namespace TaskManagement.API.Middelware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate request;
        private readonly ILogger<ErrorHandlingMiddleware> logger;

        public ErrorHandlingMiddleware(RequestDelegate _request, ILogger<ErrorHandlingMiddleware> _logger)
        {
            request = _request;
            logger = _logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                
                await request(context);
              
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var status = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                ConflictException => StatusCodes.Status409Conflict,
                ServerInternalException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };
            var errorResponse = new ErrorResponse
            {
                StatusCode = status,
                Message = status == StatusCodes.Status500InternalServerError ? "Internal Server Error" : ex.Message,
                Success = false
            };
            logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = status;
          
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Ensures camelCase formatting for JSON properties to help frontend to deal with json data
            };
            var jsonResponse = JsonSerializer.Serialize(errorResponse, options);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
