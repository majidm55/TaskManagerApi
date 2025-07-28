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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetCreatedAt();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void SetCreatedAt()
    {
        var entries = ChangeTracker.Entries<TaskItem>()
            .Where(e => e.State == EntityState.Added);

        foreach (var entry in entries)
        {
            entry.Entity.CreatedAt = DateTime.UtcNow;
        }
    }
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
        //  entity.Property(e => e.CreatedAt)
        //                     .IsRequired()
        //                     .HasDefaultValueSql("datetime('now')");
      });

     modelBuilder.Entity<TaskItem>().HasData(
      new TaskItem { Id = 1, Title = "Learn ASP.NET Core", Status = TaskStatus.InProgress, CreatedAt = new DateTime(2025, 7, 23, 10, 0, 0, DateTimeKind.Utc) },
      new TaskItem { Id = 2, Title = "Build REST API", Status = TaskStatus.Pending, CreatedAt = new DateTime(2025, 7, 24, 10, 0, 0, DateTimeKind.Utc) },
      new TaskItem { Id = 3, Title = "Add Database", Status = TaskStatus.Completed, CreatedAt = new DateTime(2025, 7, 25, 10, 0, 0, DateTimeKind.Utc) },
      new TaskItem { Id = 4, Title = "Deploy Application", Status = TaskStatus.Pending, CreatedAt = new DateTime(2025, 7, 26, 10, 0, 0, DateTimeKind.Utc) },
      new TaskItem { Id = 5, Title = "Write Tests", Status = TaskStatus.Archived, CreatedAt = new DateTime(2025, 7, 27, 10, 0, 0, DateTimeKind.Utc) }
      );
    }
  }
}