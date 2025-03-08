namespace UnitTests.Features.Queries;

public sealed class GetAllExpensesQueryHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly IMapper _mapper;
    private readonly Mock<IExpensesRepository> _repository;
    private readonly Mock<IRedisCache> _redisCache;
    private readonly GetAllExpensesQueryHandler _handler;

    public GetAllExpensesQueryHandlerTests()
    {
        _cancellationToken = new();
        _mapper = new MapperConfiguration(c => c.AddProfile<ExpensesProfile>()).CreateMapper();
        _repository = new();
        _redisCache = new();

        _handler = new GetAllExpensesQueryHandler(_repository.Object, _mapper, _redisCache.Object);
    }

    [Fact]
    public async Task GetAllExpensesQueryHandler_ShouldReturnPagedListOfRequestedExpensesAsListOfExpenseDTOAndMetadata()
    {
        // Arrange
        var name = string.Empty;
        var containedWord = "am";
        var categoryId = Guid.NewGuid().ToString();
        var applicationUserId = Guid.NewGuid().ToString();
        var page = 1;
        var pageSize = 2;
        var getAllExpensesQuery = new GetAllExpensesQuery
        {
            Name = name,
            ContainedWord = containedWord,
            CategoryId = categoryId,
            ApplicationUserId = applicationUserId,
            Page = page,
            PageSize = pageSize,
        };
        var redisKey =
            $"{nameof(Expense)}#{name}#{containedWord}#{null}#{null}#{categoryId}#{applicationUserId}#{page}#{pageSize}";
        var expenses = new List<Expense>
        {
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Name 1",
                Description = "Description 1",
                Value = 1,
                Date = DateTime.UtcNow,
                CategoryId = categoryId,
                ApplicationUserId = applicationUserId,
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Name 2",
                Description = "Description 2",
                Value = 2,
                Date = DateTime.UtcNow,
                CategoryId = categoryId,
                ApplicationUserId = applicationUserId,
            },
        };
        _repository
            .Setup(mock => mock.ApplySpecification(It.IsAny<ExpensesSpecification>()))
            .Returns(expenses.BuildMock())
            .Verifiable();
        _redisCache
            .Setup(mock => mock.GetCachedData<PagedList<ExpenseDTO>>(redisKey))
            .ReturnsAsync((PagedList<ExpenseDTO>)null!);
        _redisCache
            .Setup(mock =>
                mock.SetCachedData(
                    redisKey,
                    It.IsAny<PagedList<ExpenseDTO>>(),
                    It.IsAny<DateTimeOffset>()
                )
            )
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _handler.Handle(getAllExpensesQuery, _cancellationToken);

        // Assert
        result.Items[0].Id.Should().Be(expenses[0].Id);
        result.Items[0].Name.Should().Be(expenses[0].Name);
        result.Items[0].Description.Should().Be(expenses[0].Description);
        result.Items[0].Value.Should().Be(expenses[0].Value);
        result.Items[0].Date.Should().Be(expenses[0].Date);
        result.Items[0].CategoryId.Should().Be(expenses[0].CategoryId);
        result.Items[0].ApplicationUserId.Should().Be(expenses[0].ApplicationUserId);
        result.Items[1].Id.Should().Be(expenses[1].Id);
        result.Items[1].Name.Should().Be(expenses[1].Name);
        result.Items[1].Value.Should().Be(expenses[1].Value);
        result.Items[1].Date.Should().Be(expenses[1].Date);
        result.Items[1].CategoryId.Should().Be(expenses[1].CategoryId);
        result.Items[1].ApplicationUserId.Should().Be(expenses[1].ApplicationUserId);
        result.TotalCount.Should().Be(expenses.Count);
        result.Page.Should().Be(page);
        result.PageSize.Should().Be(pageSize);
        result.HasPreviousPage.Should().Be(false);
        result.HasNextPage.Should().Be(false);
        _repository.Verify(
            mock => mock.ApplySpecification(It.IsAny<ExpensesSpecification>()),
            Times.Once
        );
        _redisCache.Verify(mock => mock.GetCachedData<PagedList<ExpenseDTO>>(redisKey), Times.Once);
        _redisCache.Verify(
            mock =>
                mock.SetCachedData(
                    redisKey,
                    It.IsAny<PagedList<ExpenseDTO>>(),
                    It.IsAny<DateTimeOffset>()
                ),
            Times.Once
        );
    }
}
