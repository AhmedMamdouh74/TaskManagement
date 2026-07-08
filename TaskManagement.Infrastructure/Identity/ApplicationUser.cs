using Microsoft.AspNetCore.Identity;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Soft Delete
        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
