namespace UnitTests.Features.Commands;

public sealed class DeleteExpenseCommandHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IExpensesRepository> _repository;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly DeleteExpenseCommandHandler _handler;

    public DeleteExpenseCommandHandlerTests()
    {
        _cancellationToken = new();
        _repository = new();
        _uow = new();

        _handler = new DeleteExpenseCommandHandler(_repository.Object, _uow.Object);
    }

    [Theory]
    [MemberData(nameof(ExpensesMock.GetExpenses), MemberType = typeof(ExpensesMock))]
    public async Task DeleteExpenseCommandHandler_ShouldDeleteExpense(IEnumerable<Expense> expenses)
    {
        // Arrange
        var request = new DeleteExpenseCommand(expenses.First().Id);
        _repository
            .Setup(mock => mock.DeleteAsync(It.IsAny<string>(), _cancellationToken))
            .Verifiable();
        _uow.Setup(mock => mock.SaveChangesAsync(_cancellationToken)).Verifiable();

        // Act
        await _handler.Handle(request, _cancellationToken);

        // Assert
        _repository.Verify(
            mock => mock.DeleteAsync(It.IsAny<string>(), _cancellationToken),
            Times.Once
        );
        _uow.Verify(mock => mock.SaveChangesAsync(_cancellationToken), Times.Once);
    }
}
