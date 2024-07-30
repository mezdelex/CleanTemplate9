using Application.Categories.GetAsync;
using Application.Contexts;
using Domain.Categories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTests.Categories.GetAsync;

public sealed class GetCategoryQueryHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<DbSet<Category>> _dbSet;
    private readonly Mock<IApplicationDbContext> _context;
    private readonly GetCategoryQueryHandler _handler;

    public GetCategoryQueryHandlerTests()
    {
        _cancellationToken = new();
        _dbSet = new();
        _context = new();

        _handler = new GetCategoryQueryHandler(_context.Object);
    }

    [Fact]
    public async Task Handle_ValidIdGetCategoryQuery_ShouldReturnRequestedCategoryAsCategoryDTOAsync()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var getCategoryQuery = new GetCategoryQuery(guid);
        var categories = new List<Category> { new(guid, "Name 1", "Description 1") };
        _dbSet
            .As<IAsyncEnumerable<Category>>()
            .Setup(mock => mock.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Category>(categories.AsQueryable().GetEnumerator()));
        _dbSet
            .As<IQueryable<Category>>()
            .Setup(mock => mock.Provider)
            .Returns(new TestAsyncQueryProvider<Category>(categories.AsQueryable().Provider));
        _dbSet
            .As<IQueryable<Category>>()
            .Setup(mock => mock.Expression)
            .Returns(categories.AsQueryable().Expression);
        _dbSet
            .As<IQueryable<Category>>()
            .Setup(mock => mock.ElementType)
            .Returns(categories.AsQueryable().ElementType);
        _dbSet
            .As<IQueryable<Category>>()
            .Setup(mock => mock.GetEnumerator())
            .Returns(categories.AsQueryable().GetEnumerator());
        _context.Setup(mock => mock.Categories).Returns(_dbSet.Object).Verifiable();

        // Act
        var result = await _handler.Handle(getCategoryQuery, _cancellationToken);

        // Assert
        result.Id.Should().Be(categories[0].Id);
        result.Name.Should().Be(categories[0].Name);
        result.Description.Should().Be(categories[0].Description);
        _context.Verify();
    }
}
