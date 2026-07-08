using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.Admin.DTOs.Requests;
using TaskManagement.Application.Features.Admin.Interfaces;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _adminService.GetUsersAsync());
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateUser(CreateUserRequestDto request)
    {
        var user = await _adminService.CreateUserAsync(request);

        return Ok(user);
    }

    [HttpDelete("users/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _adminService.DeleteUserAsync(id);

        return NoContent();
    }
}