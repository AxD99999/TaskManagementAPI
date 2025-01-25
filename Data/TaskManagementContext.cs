using Microsoft.EntityFrameworkCore;

public class TaskManagementContext : DbContext
{
    public TaskManagementContext(DbContextOptions<TaskManagementContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Task> Tasks { get; set; }
}