using Application.Contexts;
using Domain.Categories;

namespace Infrastructure.Repositories;

public sealed class CategoriesRepository : ICategoriesRepository
{
    private readonly IApplicationDbContext _context;

    public CategoriesRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task PatchAsync(Category category, CancellationToken cancellationToken)
    {
        var foundCategory = await _context.Categories.FindAsync(category.Id, cancellationToken);

        if (foundCategory == null)
            throw new CategoryNotFoundException(category.Id);

        foundCategory.Name = category.Name;
        foundCategory.Description = category.Description;
    }

    public async Task PostAsync(Category category, CancellationToken cancellationToken) =>
        await _context.Categories.AddAsync(category, cancellationToken);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundCategory = await _context.Categories.FindAsync(id, cancellationToken);

        if (foundCategory == null)
            throw new CategoryNotFoundException(id);

        _context.Categories.Remove(foundCategory);
    }
}
