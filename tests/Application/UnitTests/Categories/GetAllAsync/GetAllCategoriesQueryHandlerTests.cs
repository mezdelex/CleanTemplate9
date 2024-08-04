using Application.Categories.GetAllAsync;
using Application.Categories.Shared;
using Application.Contexts;
using Domain.Cache;
using Domain.Categories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using static Domain.Extensions.Collections.Collections;

namespace Application.UnitTests.Categories.GetAllAsync;

public sealed class GetAllCategoriesQueryHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<DbSet<Category>> _dbSet;
    private readonly Mock<IApplicationDbContext> _context;
    private readonly Mock<IRedisCache> _redisCache;
    private readonly GetAllCategoriesQueryHandler _handler;

    public GetAllCategoriesQueryHandlerTests()
    {
        _cancellationToken = new();
        _dbSet = new();
        _context = new();
        _redisCache = new();

        _handler = new GetAllCategoriesQueryHandler(_context.Object, _redisCache.Object);
    }

    [Fact]
    public async Task GetAllCategoriesQueryHandler_ShouldReturnPagedListOfRequestedCategoriesAsListOfCategoryDTOAndMetadata()
    {
        // Arrange
        var page = 1;
        var pageSize = 2;
        var getAllCategoriesQuery = new GetAllCategoriesQuery(page, pageSize);
        var redisKey = $"{nameof(GetAllCategoriesQuery)}#{page}#{pageSize}";
        var categories = new List<Category>
        {
            new(Guid.NewGuid(), "Name 1", "Description 1"),
            new(Guid.NewGuid(), "Name 2", "Description 2")
        };
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
        _redisCache
            .Setup(mock => mock.GetCachedData<PagedList<CategoryDTO>>(redisKey))
            .ReturnsAsync((PagedList<CategoryDTO>)null!);
        _redisCache
            .Setup(mock =>
                mock.SetCachedData(
                    redisKey,
                    It.IsAny<PagedList<CategoryDTO>>(),
                    It.IsAny<DateTimeOffset>()
                )
            )
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(getAllCategoriesQuery, _cancellationToken);

        // Assert
        result.Items[0].Id.Should().Be(categories[0].Id);
        result.Items[0].Name.Should().Be(categories[0].Name);
        result.Items[0].Description.Should().Be(categories[0].Description);
        result.Items[1].Id.Should().Be(categories[1].Id);
        result.Items[1].Name.Should().Be(categories[1].Name);
        result.Items[1].Description.Should().Be(categories[1].Description);
        result.TotalCount.Should().Be(categories.Count);
        result.Page.Should().Be(page);
        result.PageSize.Should().Be(pageSize);
        result.HasPreviousPage.Should().Be(false);
        result.HasNextPage.Should().Be(false);
        _context.Verify();
        _redisCache.Verify(
            mock => mock.GetCachedData<PagedList<CategoryDTO>>(redisKey),
            Times.Once
        );
        _redisCache.Verify(
            mock =>
                mock.SetCachedData(
                    redisKey,
                    It.IsAny<PagedList<CategoryDTO>>(),
                    It.IsAny<DateTimeOffset>()
                ),
            Times.Once
        );
    }
}
