namespace UnitTests.Features.Queries;

public sealed class GetExpenseQueryHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly IMapper _mapper;
    private readonly Mock<IExpensesRepository> _repository;
    private readonly GetExpenseQueryHandler _handler;

    public GetExpenseQueryHandlerTests()
    {
        _cancellationToken = new();
        _mapper = new MapperConfiguration(c => c.AddProfile<ExpensesProfile>()).CreateMapper();
        _repository = new();
        _handler = new GetExpenseQueryHandler(_repository.Object, _mapper);
    }

    [Theory]
    [MemberData(nameof(ExpensesMock.GetExpenses), MemberType = typeof(ExpensesMock))]
    public async Task Handle_ValidIdGetExpenseQuery_ShouldReturnRequestedExpenseAsExpenseDTOAsync(
        IEnumerable<Expense> expenses
    )
    {
        // Arrange
        var request = new GetExpenseQuery(expenses.First().Id);
        _repository
            .Setup(mock =>
                mock.GetBySpecAsync(It.IsAny<ExpensesSpecification>(), _cancellationToken)
            )
            .ReturnsAsync(expenses.First())
            .Verifiable();

        // Act
        var result = await _handler.Handle(request, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<ExpenseDTO>(expenses.First()));
        _repository.Verify(
            mock => mock.GetBySpecAsync(It.IsAny<ExpensesSpecification>(), _cancellationToken),
            Times.Once
        );
    }
}
