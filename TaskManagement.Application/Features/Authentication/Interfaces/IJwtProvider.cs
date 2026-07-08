using TaskManagement.Application.Common.Models;

namespace TaskManagement.Application.Features.Authentication.Interfaces;

public interface IJwtProvider
{
    Task<JwtTokenResult> GenerateTokenAsync(JwtUser user);
}