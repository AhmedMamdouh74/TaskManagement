using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.API.Responses;
using TaskManagement.Application.Common.Exceptions;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public abstract class BaseAPIController : ControllerBase
    {
        protected ActionResult<T> Success<T>(T data, string message = "Request was successful.")
            => Ok(ApiResponse<T>.CreateSuccessResponse(data, message));
        protected ActionResult Error(string message, int statusCode = 400)
        {
            var errorResponse = new ErrorResponse
            {
                Message = message,
                StatusCode = statusCode,
                Success = false
            };
            return StatusCode(statusCode, errorResponse);
        }
        protected ActionResult<T> CreatedSuccess<T>(T data,string message = "Created successfully.")
        {
            return StatusCode(StatusCodes.Status201Created,
                ApiResponse<T>.CreateSuccessResponse(data, message));
        }

        protected IActionResult NoContentSuccess()
        {
            return NoContent();
        }
        protected int UserId
        {
            get
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedException("User is not authenticated.");

                return int.Parse(userId);
            }
        }
    }
}
