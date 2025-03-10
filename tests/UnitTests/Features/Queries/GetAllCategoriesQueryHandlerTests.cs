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
        _mapper = new MapperConfiguration(c =>
        {
            c.AddProfile<CategoriesProfile>();
            c.AddProfile<ExpensesProfile>();
        }).CreateMapper();
        _repository = new();
        _redisCache = new();

        _handler = new GetAllCategoriesQueryHandler(
            _repository.Object,
            _mapper,
            _redisCache.Object
        );
    }

    [Theory]
    [MemberData(nameof(CategoriesMock.GetCategories), MemberType = typeof(CategoriesMock))]
    public async Task GetAllCategoriesQueryHandler_ShouldReturnPagedListOfRequestedCategoriesAsListOfCategoryDTOAndMetadata(
        IEnumerable<Category> categories
    )
    {
        // Arrange
        var request = new GetAllCategoriesQuery
        {
            Name = categories.First().Name,
            ContainedWord = categories.First().Name,
            Page = 1,
            PageSize = categories.Count(),
        };
        var redisKey =
            $"{nameof(Category)}#{request.Name}#{request.ContainedWord}#{request.Page}#{request.PageSize}";
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
        var result = await _handler.Handle(request, _cancellationToken);

        // Assert
        result
            .Should()
            .BeEquivalentTo(
                new PagedList<CategoryDTO>(
                    [.. categories.Select(_mapper.Map<CategoryDTO>)],
                    1,
                    categories.Count(),
                    categories.Count(),
                    false,
                    false
                )
            );
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
