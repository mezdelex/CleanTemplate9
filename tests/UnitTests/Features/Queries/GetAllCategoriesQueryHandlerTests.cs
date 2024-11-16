using Application.Profiles;

namespace UnitTests.Features.Queries;

public sealed class GetAllCategoriesQueryHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly IMapper _mapper;
    private readonly Mock<ICategoriesRepository> _repository;
    private readonly Mock<IRedisCache> _redisCache;
    private readonly GetAllCategoriesQueryHandler _handler;

    public GetAllCategoriesQueryHandlerTests()
    {
        _cancellationToken = new();
        _mapper = new MapperConfiguration(c => c.AddProfile<CategoriesProfile>()).CreateMapper();
        _repository = new();
        _redisCache = new();

        _handler = new GetAllCategoriesQueryHandler(
            _repository.Object,
            _mapper,
            _redisCache.Object
        );
    }

    [Fact]
    public async Task GetAllCategoriesQueryHandler_ShouldReturnPagedListOfRequestedCategoriesAsListOfCategoryDTOAndMetadata()
    {
        // Arrange
        var name = string.Empty;
        var containedWord = "am";
        var page = 1;
        var pageSize = 2;
        var getAllCategoriesQuery = new GetAllCategoriesQuery
        {
            Name = name,
            ContainedWord = containedWord,
            Page = page,
            PageSize = pageSize,
        };
        var redisKey = $"{nameof(GetAllCategoriesQuery)}#{name}#{containedWord}#{page}#{pageSize}";
        var categories = new List<Category>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name 1",
                Description = "Description 1",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name 2",
                Description = "Description 2",
            },
        };
        _repository
            .Setup(mock => mock.ApplySpecification(It.IsAny<CategoriesSpecification>()))
            .Returns(categories.BuildMock())
            .Verifiable();
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
            .Returns(Task.CompletedTask)
            .Verifiable();

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
        _repository.Verify(
            mock => mock.ApplySpecification(It.IsAny<CategoriesSpecification>()),
            Times.Once
        );
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
