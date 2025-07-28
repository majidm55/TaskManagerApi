namespace TaskManagerApi.Models.Responses {
public class PagedResult<T>
{
  public required IEnumerable<T> Data { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; } = 0;
    public int TotalPages { get; set; } = 0;
    public bool HasPrevious { get; set; } = false;
    public bool HasNext { get; set; } = false;
}

public class GroupedTasksResponse
{
    public required PagedResult<TaskItem> Pending { get; set; }
    public required PagedResult<TaskItem> InProgress { get; set; }
    public required PagedResult<TaskItem> Completed { get; set; }
    public required PagedResult<TaskItem> Archived { get; set; }
}

}