namespace TaskManagement.Application.Common.Models;

public sealed class JwtTokenResult
{
    public string Token { get; init; } = string.Empty;

    public DateTime Expiration { get; init; }
}