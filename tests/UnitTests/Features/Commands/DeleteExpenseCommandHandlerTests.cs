namespace Application.UnitTests.Expenses.PostAsync;

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

    [Fact]
    public async Task DeleteExpenseCommandHandler_ShouldDeleteExpense()
    {
        // Arrange
        var deleteExpenseCommand = new DeleteExpenseCommand(Guid.NewGuid());
        _repository
            .Setup(mock => mock.DeleteAsync(It.IsAny<Guid>(), _cancellationToken))
            .Verifiable();
        _uow.Setup(mock => mock.SaveChangesAsync(_cancellationToken)).Verifiable();

        // Act
        await _handler.Handle(deleteExpenseCommand, _cancellationToken);

        // Assert
        _repository.Verify(
            mock => mock.DeleteAsync(It.IsAny<Guid>(), _cancellationToken),
            Times.Once
        );
        _uow.Verify(mock => mock.SaveChangesAsync(_cancellationToken), Times.Once);
    }
}
