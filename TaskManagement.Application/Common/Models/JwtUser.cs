namespace TaskManagement.Application.Common.Models;
public sealed class JwtUser
{
    public int Id { get; init; }

    public string Email { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public IEnumerable<string> Roles { get; init; } = [];
}