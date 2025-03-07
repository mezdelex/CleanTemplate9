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

    [Fact]
    public async Task Handle_ValidIdGetExpenseQuery_ShouldReturnRequestedExpenseAsExpenseDTOAsync()
    {
        // Arrange
        var guid = Guid.NewGuid().ToString();
        var getExpenseQuery = new GetExpenseQuery(guid);
        var category = new Expense
        {
            Id = guid,
            Name = "Name 1",
            Description = "Description 1",
            CategoryId = guid,
        };
        _repository
            .Setup(mock =>
                mock.GetBySpecAsync(It.IsAny<ExpensesSpecification>(), _cancellationToken)
            )
            .ReturnsAsync(category)
            .Verifiable();

        // Act
        var result = await _handler.Handle(getExpenseQuery, _cancellationToken);

        // Assert
        result.Id.Should().Be(category.Id);
        result.Name.Should().Be(category.Name);
        result.Description.Should().Be(category.Description);
        result.CategoryId.Should().Be(category.CategoryId);
        _repository.Verify(
            mock => mock.GetBySpecAsync(It.IsAny<ExpensesSpecification>(), _cancellationToken),
            Times.Once
        );
    }
}
