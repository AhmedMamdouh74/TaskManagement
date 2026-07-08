using TaskManagement.Domain.Entities;
namespace TaskManagement.Application.Interfaces.Persistence;
public interface ITaskRepository
{
    Task AddAsync(TaskItem task);

    Task<TaskItem?> GetByIdAsync(int id);

    Task<List<TaskItem>> GetByUserIdAsync(int userId);

    Task<bool> ExistsAsync(int userId, string title, DateTime date);

    void Update(TaskItem task);

    Task SaveChangesAsync();
}