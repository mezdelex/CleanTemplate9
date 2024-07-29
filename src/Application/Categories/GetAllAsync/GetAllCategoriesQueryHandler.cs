using Application.Categories.Shared;
using Application.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Domain.Extensions.Collections.Collections;

namespace Application.Categories.GetAllAsync;

public sealed class GetAllCategoriesQueryHandler
    : IRequestHandler<GetAllCategoriesQuery, PagedList<CategoryDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllCategoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<CategoryDTO>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken
    ) =>
        await _context
            .Categories.Include(c => c.Expenses)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDTO(c.Id, c.Name, c.Description, c.Expenses))
            .ToPagedListAsync(request.Page, request.PageSize, cancellationToken);
}