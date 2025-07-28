using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;
using TaskManagerApi.Models.Responses;

namespace TaskManagerApi.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<TaskItem>> ToPagedResultAsync(
            this IQueryable<TaskItem> query, 
            int pageNumber, 
            int pageSize)
        {
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(x => x.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return new PagedResult<TaskItem>
            {
                Data = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                HasPrevious = pageNumber > 1,
                HasNext = pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }
}