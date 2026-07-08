using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Features.Tasks.DTOs.Requests;

public class UpdateTaskStatusRequestDto
{
    public TaskItemStatus Status { get; set; }
}