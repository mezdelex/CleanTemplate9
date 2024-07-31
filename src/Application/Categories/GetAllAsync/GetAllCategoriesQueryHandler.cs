using Application.Categories.Shared;
using Application.Contexts;
using Domain.Cache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Domain.Extensions.Collections.Collections;

namespace Application.Categories.GetAllAsync;

public sealed class GetAllCategoriesQueryHandler
    : IRequestHandler<GetAllCategoriesQuery, PagedList<CategoryDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IRedisCache _redisCache;

    public GetAllCategoriesQueryHandler(IApplicationDbContext context, IRedisCache redisCache)
    {
        _context = context;
        _redisCache = redisCache;
    }

    public async Task<PagedList<CategoryDTO>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        var redisKey = $"{nameof(GetAllCategoriesQuery)}#{request.Page}#{request.PageSize}";
        var cachedGetAllCategoriesQuery = await _redisCache.GetCachedData<PagedList<CategoryDTO>>(
            redisKey
        );
        if (cachedGetAllCategoriesQuery != null)
            return cachedGetAllCategoriesQuery;

        var pagedCategories = await _context
            .Categories.Include(c => c.Expenses)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDTO(c.Id, c.Name, c.Description, c.Expenses))
            .ToPagedListAsync(request.Page, request.PageSize, cancellationToken);

        await _redisCache.SetCachedData<PagedList<CategoryDTO>>(
            redisKey,
            pagedCategories,
            DateTimeOffset.Now.AddMinutes(5)
        );

        return pagedCategories;
    }
}
