using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Features.Admin.DTOs.Requests;
using TaskManagement.Application.Features.Admin.DTOs.Responses;
using TaskManagement.Application.Features.Admin.Interfaces;
using TaskManagement.Infrastructure.Identity;

namespace TaskManagement.Infrastructure.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserResponseDto>> GetUsersAsync()
    {
        return await _userManager.Users
            .Select(user => new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email!
            })
            .ToListAsync();
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
            throw new InvalidOperationException("User already exists.");

        var user = new ApplicationUser
        {
            Name = request.Name,
            UserName = request.Email,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, "User");

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email!
        };
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
            throw new InvalidOperationException("User not found.");

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}