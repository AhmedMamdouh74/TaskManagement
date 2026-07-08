using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Controllers;
using TaskManagement.Application.Features.Admin.DTOs.Requests;
using TaskManagement.Application.Features.Admin.DTOs.Responses;
using TaskManagement.Application.Features.Admin.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : BaseAPIController
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
    {
        var users = await _adminService.GetUsersAsync();

        return Success(users, "Users retrieved successfully.");
    }

    [HttpPost("users")]
    public async Task<ActionResult<UserResponseDto>> CreateUser(
        [FromBody] CreateUserRequestDto request)
    {
        var user = await _adminService.CreateUserAsync(request);

        return CreatedSuccess(user, "User created successfully.");
    }

    [HttpDelete("users/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _adminService.DeleteUserAsync(id);

        return NoContent();
    }
}