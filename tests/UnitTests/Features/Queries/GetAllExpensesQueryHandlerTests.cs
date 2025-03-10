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

    [Theory]
    [MemberData(nameof(ExpensesMock.GetExpenses), MemberType = typeof(ExpensesMock))]
    public async Task GetAllExpensesQueryHandler_ShouldReturnPagedListOfRequestedExpensesAsListOfExpenseDTOAndMetadata(
        IEnumerable<Expense> expenses
    )
    {
        // Arrange
        var request = new GetAllExpensesQuery
        {
            Name = expenses.First().Name,
            ContainedWord = expenses.First().Name,
            CategoryId = expenses.First().CategoryId,
            ApplicationUserId = expenses.First().ApplicationUserId,
            Page = 1,
            PageSize = expenses.Count(),
        };
        var redisKey =
            $"{nameof(Expense)}#{request.Name}#{request.ContainedWord}#{request.MinDate}#{request.MaxDate}#{request.CategoryId}#{request.ApplicationUserId}#{request.Page}#{request.PageSize}";
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
        var result = await _handler.Handle(request, _cancellationToken);

        // Assert
        result
            .Should()
            .BeEquivalentTo(
                new PagedList<ExpenseDTO>(
                    [.. expenses.Select(_mapper.Map<ExpenseDTO>)],
                    1,
                    expenses.Count(),
                    expenses.Count(),
                    false,
                    false
                )
            );
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
