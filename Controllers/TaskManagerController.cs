using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Models;
using TaskStatusEnum = TaskManagerApi.Models.TaskStatus;
using TaskManagerApi.DTO;
using TaskManagerApi.Data;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models.Responses;
using TaskManagerApi.Extensions;


namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskManagerController : ControllerBase
{
    private readonly TaskItemDbContext _context;
    private readonly ILogger<TaskManagerController> _logger;
    public TaskManagerController(TaskItemDbContext context, ILogger<TaskManagerController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<GroupedTasksResponse>> GetAllTasks([FromQuery] PaginationParameters paginationParams)
    {
        var grouped = new GroupedTasksResponse
        {
            Pending = await _context.TaskItems
                .Where(t => t.Status == TaskStatusEnum.Pending)
                .ToPagedResultAsync(paginationParams.PageNumber, paginationParams.PageSize),
                
            InProgress = await _context.TaskItems
                .Where(t => t.Status == TaskStatusEnum.InProgress)
                .ToPagedResultAsync(paginationParams.PageNumber, paginationParams.PageSize),
                
            Completed = await _context.TaskItems
                .Where(t => t.Status == TaskStatusEnum.Completed)
                .ToPagedResultAsync(paginationParams.PageNumber, paginationParams.PageSize),
                
            Archived = await _context.TaskItems
                .Where(t => t.Status == TaskStatusEnum.Archived)
                .ToPagedResultAsync(paginationParams.PageNumber, paginationParams.PageSize)
        };
        
        return Ok(grouped);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<List<TaskItem>>> GetTaskById(int id)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task == null)
        {
            return NotFound($"Task with ID {id} not found.");
        }
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskItemCreateDto taskDto)
    {
        var taskItem = new TaskItem
        {
            Title = taskDto.Title,
            Status = taskDto.Status
        };

        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateTask), new { id = taskItem.Id }, taskItem);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItemUpdateDto updateDto)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task == null)
        {
            return NotFound($"Task with ID {id} not found.");
        }

        if (updateDto.Title != null)
        {
            task.Title = updateDto.Title;
        }

        if (updateDto.Status.HasValue)
        {
            task.Status = (TaskStatusEnum)updateDto.Status;
        }
        try
        {
            await _context.SaveChangesAsync();
            return Ok(task);
        }
        catch (DbUpdateException ex)
        {
            return BadRequest($"Failed to update task: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task == null)
        {
            return NotFound($"Task with ID {id} not found.");
        }

        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("analytics")]
    public async Task<ActionResult<GroupedAnalyticsResponse>> GetTaskAnalytics()
    {
        var analytics = await _context.TaskItems.ToAnalyticsAsync();

        return Ok(analytics);
    }



};

