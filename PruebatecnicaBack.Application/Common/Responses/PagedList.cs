using Microsoft.EntityFrameworkCore;

namespace PruebatecnicaBack.Contracts.Common;

public class PagedList<T>
{
    private PagedList(List<T> items, int page, int pageSize, int totalCount, int totalPages) 
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }
    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPreviousPage => Page > 1;
    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
    {
        int totalCount = await query.CountAsync();
        double totalPages = Math.Ceiling((double)totalCount / pageSize);
        var items = await query.Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();
        return new(items, page, pageSize, totalCount, (int)totalPages);
    }
}
