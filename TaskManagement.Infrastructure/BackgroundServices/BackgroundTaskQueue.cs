using System.Threading.Channels;
using TaskManagement.Application.Interfaces.Background;

namespace TaskManagement.Infrastructure.BackgroundServices;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<int> _queue = Channel.CreateUnbounded<int>();

    public async ValueTask QueueTaskAsync(int taskId)
    {
        await _queue.Writer.WriteAsync(taskId);
    }

    public async ValueTask<int> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}