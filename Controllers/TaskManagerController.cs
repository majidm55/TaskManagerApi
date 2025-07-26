using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Models;
using TaskStatusEnum = TaskManagerApi.Models.TaskStatus;
using TaskManagerApi.Data;
using Microsoft.EntityFrameworkCore;


namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAllTasks()
    {
        var tasks = await _context.TaskItems.ToListAsync();
        return Ok(tasks);
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

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByStatus(TaskStatusEnum status)
    {
        var tasks = await _context.TaskItems
            .Where(t => t.Status == status)
            .ToListAsync();
        
        return Ok(tasks);
    }



};

