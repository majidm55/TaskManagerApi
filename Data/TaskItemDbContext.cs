using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;
using TaskStatus = TaskManagerApi.Models.TaskStatus;

namespace TaskManagerApi.Data
{
  public class TaskItemDbContext : DbContext
  {
    public TaskItemDbContext(DbContextOptions<TaskItemDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<TaskItem>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
        entity.Property(e => e.Status)
                    .HasConversion<int>();
      });

      modelBuilder.Entity<TaskItem>().HasData(
          new TaskItem { Id = 1, Title = "Learn ASP.NET Core", Status = TaskStatus.InProgress },
          new TaskItem { Id = 2, Title = "Build REST API", Status = TaskStatus.Pending },
          new TaskItem { Id = 3, Title = "Add Database", Status = TaskStatus.Completed },
          new TaskItem { Id = 4, Title = "Deploy Application", Status = TaskStatus.Pending },
          new TaskItem { Id = 5, Title = "Write Tests", Status = TaskStatus.Archived }
      );
    }
  }
}