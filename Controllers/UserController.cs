using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly TaskManagementContext _context;

    public UsersController(TaskManagementContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            // Return validation error response
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { error = string.Join(", ", errors) });
        }

        // Check if email is unique
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
        {
            // Return validation error response
            return BadRequest(new { error = "Email already exists" });
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Return success response
        return Ok(new { message = "User created successfully", userId = user.UserId });
    }
}