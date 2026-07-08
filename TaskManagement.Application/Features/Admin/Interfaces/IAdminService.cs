using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Features.Admin.DTOs.Requests;
using TaskManagement.Application.Features.Admin.DTOs.Responses;

namespace TaskManagement.Application.Features.Admin.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<UserResponseDto>> GetUsersAsync();

        Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto request);

        Task DeleteUserAsync(int id);
    }
}
