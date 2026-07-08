namespace TaskManagement.Application.Interfaces.Background;

public interface IBackgroundTaskQueue
{
    ValueTask QueueTaskAsync(int taskId);

    ValueTask<int> DequeueAsync(CancellationToken cancellationToken);
}