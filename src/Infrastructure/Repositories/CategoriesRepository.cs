using Application.Contexts;
using Domain.Categories;

namespace Infrastructure.Repositories;

public class CategoriesRepository : ICategoriesRepository
{
    private readonly IApplicationDbContext _context;

    public CategoriesRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task PatchAsync(Guid Id, Category category, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public Task PostAsync(Category category, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid Id, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}