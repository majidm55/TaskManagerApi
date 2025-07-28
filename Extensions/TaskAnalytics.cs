
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;
using TaskManagerApi.Models.Responses;
using TaskStatus = TaskManagerApi.Models.TaskStatus;

namespace TaskManagerApi.Extensions {
  public static class TaskAnalyticsExtensions
  {
      public static async Task<GroupedAnalyticsResponse> ToAnalyticsAsync(this IQueryable<TaskItem> query)
      {
          var grouped = await query
              .GroupBy(t => t.Status)
              .Select(g => new { Status = g.Key, Count = g.Count() })
              .ToListAsync();

          return new GroupedAnalyticsResponse
          {
              Pending = new AnalyticsResult { Value = grouped.FirstOrDefault(x => x.Status == TaskStatus.Pending)?.Count ?? 0, Name = "Pending" },
              InProgress = new AnalyticsResult { Value = grouped.FirstOrDefault(x => x.Status == TaskStatus.InProgress)?.Count ?? 0, Name = "In Progress" },
              Completed = new AnalyticsResult { Value = grouped.FirstOrDefault(x => x.Status == TaskStatus.Completed)?.Count ?? 0, Name = "Completed" },
              Archived = new AnalyticsResult { Value = grouped.FirstOrDefault(x => x.Status == TaskStatus.Archived)?.Count ?? 0 , Name = "Archived"}
          };
      }
  }

}