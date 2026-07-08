using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Features.Tasks.DTOs.Requests;

public class CreateTaskRequestDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TaskPriority Priority { get; set; }
}