namespace Application.UnitTests.Expenses.PostAsync;

public sealed class PatchExpenseCommandHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IValidator<PatchExpenseCommand>> _validator;
    private readonly Mock<IExpensesRepository> _repository;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IEventBus> _eventBus;
    private readonly PatchExpenseCommandHandler _handler;

    public PatchExpenseCommandHandlerTests()
    {
        _cancellationToken = new();
        _validator = new();
        _repository = new();
        _uow = new();
        _eventBus = new();

        _handler = new PatchExpenseCommandHandler(
            _validator.Object,
            _repository.Object,
            _uow.Object,
            _eventBus.Object
        );
    }

    [Fact]
    public async Task PatchExpenseCommandHandler_ShouldPatchExpenseAndPublishEventAsync()
    {
        // Arrange
        var patchExpenseCommand = new PatchExpenseCommand(
            Guid.NewGuid(),
            "Expense 1 name",
            "Expense 1 description",
            1,
            new Guid()
        );
        _validator
            .Setup(mock => mock.ValidateAsync(It.IsAny<PatchExpenseCommand>(), _cancellationToken))
            .ReturnsAsync(new ValidationResult())
            .Verifiable();
        _repository
            .Setup(mock => mock.PatchAsync(It.IsAny<Expense>(), _cancellationToken))
            .Verifiable();
        _uow.Setup(mock => mock.SaveChangesAsync(_cancellationToken)).Verifiable();
        _eventBus
            .Setup(mock => mock.PublishAsync(It.IsAny<PatchedExpenseEvent>(), _cancellationToken))
            .Verifiable();

        // Act
        await _handler.Handle(patchExpenseCommand, _cancellationToken);

        // Assert
        _validator.Verify(
            mock => mock.ValidateAsync(It.IsAny<PatchExpenseCommand>(), _cancellationToken),
            Times.Once
        );
        _repository.Verify(
            mock => mock.PatchAsync(It.IsAny<Expense>(), _cancellationToken),
            Times.Once
        );
        _uow.Verify(mock => mock.SaveChangesAsync(_cancellationToken), Times.Once);
        _eventBus.Verify(
            mock => mock.PublishAsync(It.IsAny<PatchedExpenseEvent>(), _cancellationToken),
            Times.Once
        );
    }
}
