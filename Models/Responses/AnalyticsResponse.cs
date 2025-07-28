namespace TaskManagerApi.Models.Responses {
  public class AnalyticsResult
  {
    public  int Value { get; set; } = 0;
    public required string Name {get; set;}

}

public class GroupedAnalyticsResponse
{
    public required AnalyticsResult Pending { get; set; }
    public required AnalyticsResult InProgress { get; set; }
    public required AnalyticsResult Completed { get; set; }
    public required AnalyticsResult Archived { get; set; }
}

}