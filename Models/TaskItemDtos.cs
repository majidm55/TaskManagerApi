namespace TaskManagerApi.Models
{
    public class TaskItemUpdateDto
    {
        public string? Title { get; set; }
        public bool? IsComplete { get; set; }
        public TaskStatus? Status { get; set; }
    }

    public class TaskItemCreateDto
    {
        public required string Title { get; set; }
        public bool IsComplete { get; set; } = false;
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
    }
}

