using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models.Responses;

namespace TaskManagerApi.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query, 
            int pageNumber, 
            int pageSize)
        {
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return new PagedResult<T>
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