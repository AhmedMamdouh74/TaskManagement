namespace TaskManagement.Application.Features.Authentication.DTOs.Responses;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }
}