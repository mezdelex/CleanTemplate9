using Application.Categories.GetAllAsync;
using Application.Contexts;
using Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.Categories.GetAllAsync;

public sealed class GetAllCategoriesQueryHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly GetAllCategoriesQueryHandler _handler;
    private readonly Mock<DbSet<Category>> _dbSet;
    private readonly Mock<IApplicationDbContext> _context;

    public GetAllCategoriesQueryHandlerTests()
    {
        _cancellationToken = new();
        _context = new();
        _dbSet = new();

        _handler = new GetAllCategoriesQueryHandler(_context.Object);
    }

    /* TODO: continue here */
    /* TODO: continue here */
    /* TODO: continue here */
    /* TODO: continue here */
    /* TODO: continue here */
}
