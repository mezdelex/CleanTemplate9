namespace Domain.Extensions;

public static class Collections
{
    public sealed record PagedList<T>(
        List<T> Items,
        int Page,
        int PageSize,
        int TotalCount,
        bool HasPreviousPage,
        bool HasNextPage
    );

    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        var hasPreviousPage = page > 1;
        var hasNextPage = page * pageSize < totalCount;

        return new PagedList<T>(items, page, pageSize, totalCount, hasPreviousPage, hasNextPage);
    }
}
