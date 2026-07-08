using TaskManagement.Application.Features.Tasks.DTOs.Requests;
using TaskManagement.Application.Features.Tasks.DTOs.Responses;

namespace TaskManagement.Application.Features.Tasks.Interfaces;

public interface ITaskService
{
    Task<TaskResponseDto> CreateAsync(int userId, CreateTaskRequestDto request);

    Task<IEnumerable<TaskResponseDto>> GetAllAsync(int userId);

    Task<TaskResponseDto> GetByIdAsync(int id, int userId);

    Task UpdateStatusAsync(int id, int userId, UpdateTaskStatusRequestDto request);
}