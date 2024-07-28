using Application.Contexts;
using Application.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Domain.Extensions.Collections.Collections;

namespace Application.Categories.GetAllAsync;

public class GetAllCategoriesQueryAsyncHandler
    : IRequestHandler<GetAllCategoriesQueryAsync, PagedList<CategoryDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllCategoriesQueryAsyncHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<CategoryDTO>> Handle(
        GetAllCategoriesQueryAsync request,
        CancellationToken cancellationToken
    ) =>
        await _context
            .Categories.Include(c => c.Expenses)
            .Select(c => new CategoryDTO(c.Id, c.Name, c.Description, c.Expenses))
            .ToPagedListAsync(request.Page, request.PageSize, cancellationToken);
}
