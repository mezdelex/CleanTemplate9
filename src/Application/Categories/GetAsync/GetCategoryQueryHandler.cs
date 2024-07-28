using Application.Categories.Shared;
using Application.Contexts;
using Domain.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.GetAsync;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDTO> Handle(
        GetCategoryQuery request,
        CancellationToken cancellationToken
    )
    {
        var category =
            await _context
                .Categories.Include(c => c.Expenses)
                .FirstOrDefaultAsync(c => c.Id == request.id, cancellationToken)
            ?? throw new CategoryNotFoundException(request.id);

        return new CategoryDTO(
            category.Id,
            category.Name,
            category.Description,
            category?.Expenses
        );
    }
}
