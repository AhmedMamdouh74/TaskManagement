using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.API.Controllers;
using TaskManagement.Application.Features.Authentication.DTOs.Requests;
using TaskManagement.Application.Features.Authentication.DTOs.Responses;
using TaskManagement.Application.Features.Authentication.Interfaces;

namespace TaskManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : BaseAPIController
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);

        return CreatedSuccess(result, "User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        return Success(result, "Login successful.");
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserProfileResponseDto>> GetCurrentUser()
    {
       

        var result = await _authService.GetCurrentUserAsync(UserId);

        return Success(result);
    }
}