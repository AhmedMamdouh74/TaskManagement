using Microsoft.AspNetCore.Http;

namespace TaskManagement.API.Middelware
{
    public class NotFoundEndpointMiddleware
    {
        private readonly RequestDelegate request;
        private readonly ILogger<NotFoundEndpointMiddleware> logger;

        public NotFoundEndpointMiddleware(RequestDelegate _request, ILogger<NotFoundEndpointMiddleware> _logger)
        {
            request = _request;
            logger = _logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            
            await request(context);
           
            if (context.Response.StatusCode == StatusCodes.Status404NotFound&&!context.Response.HasStarted)
            {
                var errorResponse = new
                {
                    message = "The requested endpoint was not found.",
                    statusCode = StatusCodes.Status404NotFound,
                    success = false
                };
              //  logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
           
        }
    }
}
