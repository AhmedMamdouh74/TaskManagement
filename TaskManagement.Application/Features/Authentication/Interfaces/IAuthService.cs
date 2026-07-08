using TaskManagement.Application.Features.Authentication.DTOs.Requests;
using TaskManagement.Application.Features.Authentication.DTOs.Responses;

namespace TaskManagement.Application.Features.Authentication.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

    Task<UserProfileResponseDto> GetCurrentUserAsync(int userId);
}