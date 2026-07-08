using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Interfaces.Background;
using TaskManagement.Application.Interfaces.Persistence;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Infrastructure.BackgroundServices;

public class TaskProcessingBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IBackgroundTaskQueue _queue;
    private readonly ILogger<TaskProcessingBackgroundService> _logger;

    public TaskProcessingBackgroundService(
        IServiceScopeFactory scopeFactory,
        IBackgroundTaskQueue queue,
        ILogger<TaskProcessingBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _queue = queue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Task Processing Background Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var taskId = await _queue.DequeueAsync(stoppingToken);

                _logger.LogInformation("Processing task {TaskId}", taskId);

                // Simulate long-running processing
                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);

                using var scope = _scopeFactory.CreateScope();

                var repository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

                var task = await repository.GetByIdAsync(taskId);

                if (task is null)
                {
                    _logger.LogWarning("Task {TaskId} not found.", taskId);
                    continue;
                }

                task.UpdateStatus(TaskItemStatus.InProgress);

                repository.Update(task);

                await repository.SaveChangesAsync();

                _logger.LogInformation(
                    "Task {TaskId} status updated to {Status}.",
                    taskId,
                    TaskItemStatus.InProgress);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Task Processing Background Service is stopping.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while processing a background task.");
            }
        }

        _logger.LogInformation("Task Processing Background Service stopped.");
    }
}