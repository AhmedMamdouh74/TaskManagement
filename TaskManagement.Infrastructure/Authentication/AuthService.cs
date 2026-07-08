using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Models;
using TaskManagement.Application.Features.Authentication.DTOs.Requests;
using TaskManagement.Application.Features.Authentication.DTOs.Responses;
using TaskManagement.Application.Features.Authentication.Interfaces;
using TaskManagement.Infrastructure.Identity;

namespace TaskManagement.Infrastructure.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }
        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                throw new ConflictException("Email already exists.");
            }

            var user = new ApplicationUser
            {
                Name = request.Name,
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new BadRequestException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(user, "User");

            var roles = await _userManager.GetRolesAsync(user);

            var jwtUser = new JwtUser
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email!,
                Roles = roles
            };

            var token = await _jwtProvider.GenerateTokenAsync(jwtUser);

            return new AuthResponseDto
            {
                Token = token.Token,
                Expiration = token.Expiration
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                throw new UnauthorizedException("Invalid email or password.");

            var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!validPassword)
                throw new UnauthorizedException("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);

            var jwtUser = new JwtUser
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email!,
                Roles = roles
            };

            var token = await _jwtProvider.GenerateTokenAsync(jwtUser);

            return new AuthResponseDto
            {
                Token = token.Token,
                Expiration = token.Expiration
            };
        }

        public async Task<UserProfileResponseDto> GetCurrentUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                throw new NotFoundException("User not found.");
            var roles = await _userManager.GetRolesAsync(user);

            return new UserProfileResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? string.Empty
            };
        }
    }

}
