using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.Tasks.DTOs.Requests;
using TaskManagement.Application.Features.Tasks.DTOs.Responses;
using TaskManagement.Application.Features.Tasks.Interfaces;

namespace TaskManagement.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : BaseAPIController
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskRequestDto request)
    {
        var result = await _taskService.CreateAsync(UserId, request);

        return CreatedSuccess(result, "Task created successfully.");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll()
    {
        var result = await _taskService.GetAllAsync(UserId);

        return Success(result, "Tasks retrieved successfully.");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(int id)
    {
        var result = await _taskService.GetByIdAsync(id, UserId);

        return Success(result, "Task retrieved successfully.");
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateTaskStatusRequestDto request)
    {
        await _taskService.UpdateStatusAsync(id, UserId, request);

        return NoContent();
    }
}