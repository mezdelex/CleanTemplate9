using Application.Contexts;
using Application.Shared;
using Domain.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.GetAsync;

public class GetCategoryQueryAsyncHandler : IRequestHandler<GetCategoryQueryAsync, CategoryDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryQueryAsyncHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDTO> Handle(
        GetCategoryQueryAsync request,
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
