namespace TaskManagerApi.Models
{
  public enum TaskStatus
  {
    Pending,
    InProgress,
    Completed,
    Archived
  }
  public class TaskItem
  {
    public int Id { get; set; }
    public required string Title { get; set; }

    public TaskStatus Status { get; set; } = TaskStatus.Pending;
  }
  
}