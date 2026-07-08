using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; private set; }

        public string Description { get; private set; }

        public TaskItemStatus Status { get; private set; }

        public TaskPriority Priority { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public int UserId { get; private set; }

        private TaskItem() { } // EF Core

        public TaskItem(string title, string description, TaskPriority priority, int userId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.");

            if (title.Length > 100)
                throw new ArgumentException("Title cannot exceed 100 characters.");

            Title = title;
            Description = description;
            Priority = priority;
            UserId = userId;
            Status = TaskItemStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(TaskItemStatus status)
        {
            Status = status;
        }
    }
}
