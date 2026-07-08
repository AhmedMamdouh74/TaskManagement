using TaskManagement.Application.Features.Tasks.DTOs.Requests;
using TaskManagement.Application.Features.Tasks.DTOs.Responses;
using TaskManagement.Application.Features.Tasks.Interfaces;
using TaskManagement.Application.Interfaces.Persistence;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskResponseDto> CreateAsync(
        int userId,
        CreateTaskRequestDto request)
    {
        var exists = await _taskRepository.ExistsAsync(
            userId,
            request.Title,
            DateTime.UtcNow);

        if (exists)
            throw new InvalidOperationException(
                "You already have a task with the same title today.");

        var task = new TaskItem(
            request.Title,
            request.Description,
            request.Priority,
            userId);

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();

        return Map(task);
    }

    public async Task<IEnumerable<TaskResponseDto>> GetAllAsync(int userId)
    {
        var tasks = await _taskRepository.GetByUserIdAsync(userId);

        return tasks.Select(Map);
    }

    public async Task<TaskResponseDto> GetByIdAsync(
        int id,
        int userId)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if (task is null)
            throw new InvalidOperationException("Task not found.");

        if (task.UserId != userId)
            throw new UnauthorizedAccessException();

        return Map(task);
    }

    public async Task UpdateStatusAsync(
        int id,
        int userId,
        UpdateTaskStatusRequestDto request)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if (task is null)
            throw new InvalidOperationException("Task not found.");

        if (task.UserId != userId)
            throw new UnauthorizedAccessException();

        task.UpdateStatus(request.Status);

        _taskRepository.Update(task);

        await _taskRepository.SaveChangesAsync();
    }

    private static TaskResponseDto Map(TaskItem task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            CreatedAt = task.CreatedAt
        };
    }
}