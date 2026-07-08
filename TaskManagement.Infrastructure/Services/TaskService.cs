using TaskManagement.Application.Features.Tasks.DTOs.Requests;
using TaskManagement.Application.Features.Tasks.DTOs.Responses;
using TaskManagement.Application.Features.Tasks.Interfaces;
using TaskManagement.Application.Interfaces.Background;
using TaskManagement.Application.Interfaces.Cache;
using TaskManagement.Application.Interfaces.Persistence;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICacheService _cacheService;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public TaskService(ITaskRepository taskRepository, ICacheService cacheService, IBackgroundTaskQueue backgroundTaskQueue)
    {
        _taskRepository = taskRepository;
        _cacheService = cacheService;
        _backgroundTaskQueue = backgroundTaskQueue;
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

        var response = Map(task);

        await _cacheService.SetAsync(
            $"task:{userId}:{task.Id}",
            response,
            TimeSpan.FromMinutes(5));

        // Queue the task for background processing
        await _backgroundTaskQueue.QueueTaskAsync(task.Id);

        return response;
    }

    public async Task<IEnumerable<TaskResponseDto>> GetAllAsync(int userId)
    {
        var tasks = await _taskRepository.GetByUserIdAsync(userId);

        return tasks.Select(Map);
    }

    public async Task<TaskResponseDto> GetByIdAsync(int id, int userId)
    {
        var cacheKey = $"task:{userId}:{id}";

        var cachedTask =
            await _cacheService.GetAsync<TaskResponseDto>(cacheKey);

        if (cachedTask is not null)
            return cachedTask;

        var task = await _taskRepository.GetByIdAsync(id);

        if (task is null)
            throw new InvalidOperationException("Task not found.");

        if (task.UserId != userId)
            throw new UnauthorizedAccessException();

        var response = Map(task);

        await _cacheService.SetAsync(
            cacheKey,
            response,
            TimeSpan.FromMinutes(5));

        return response;
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
        await _cacheService.RemoveAsync($"task:{userId}:{id}");
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