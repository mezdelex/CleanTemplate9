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
        var foundCategory =
            await _context.Categories.FindAsync(category.Id, cancellationToken)
            ?? throw new CategoryNotFoundException(category.Id);

        _context.Categories.Entry(foundCategory).CurrentValues.SetValues(category);
        _context.Categories.Entry(foundCategory).Property(p => p.Id).IsModified = false;
    }

    public async Task PostAsync(Category category, CancellationToken cancellationToken) =>
        await _context.Categories.AddAsync(category, cancellationToken);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundCategory =
            await _context.Categories.FindAsync(id, cancellationToken)
            ?? throw new CategoryNotFoundException(id);

        _context.Categories.Remove(foundCategory);
    }
}
