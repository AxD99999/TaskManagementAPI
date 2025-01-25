using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly TaskManagementContext _context;

    public TasksController(TaskManagementContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] Task task)
    {
        if (!ModelState.IsValid)
        {
            // Return validation error response
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { error = string.Join(", ", errors) });
        }

        // Check if AssignedTo references an existing user
        var assignedUser = await _context.Users.FindAsync(task.AssignedTo);
        if (assignedUser == null)
        {
            // Return validation error response
            return BadRequest(new { error = "Assigned user does not exist" });
        }

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Return success response
        return Ok(new { message = "Task created successfully", taskId = task.TaskId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        var tasks = await _context.Tasks.Join(
            _context.Users,
            task => task.AssignedTo,
            user => user.UserId,
            (task, user) => new
            {
                task.TaskId,
                task.Title,
                task.Description,
                task.Status,
                AssignedTo = new
                {
                    user.UserId,
                    user.Name,
                },
                task.CreatedDate
            }
            )
            .ToListAsync();
        return Ok(tasks);
    }

    [HttpPut("{taskId}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int taskId, [FromBody] string status)
    {
        // Define Allowed States
        var allowedStatuses = new[] { "Pending", "In Progress", "Completed" };

        //Check if the provided status is valid
        if (!allowedStatuses.Contains(status))
        {
            return BadRequest(new { error = "Invalid status" });
        }

        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null)
            return NotFound(new { error = "Task not found" });

        task.Status = status;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Task status updated successfully" });
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task == null)
            return NotFound(new { error = "Task not found" });

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Task deleted successfully" });
    }
}