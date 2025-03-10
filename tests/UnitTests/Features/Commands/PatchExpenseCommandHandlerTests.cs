namespace UnitTests.Features.Commands;

public sealed class PatchExpenseCommandHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IValidator<PatchExpenseCommand>> _validator;
    private readonly IMapper _mapper;
    private readonly Mock<IExpensesRepository> _repository;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IRedisCache> _redisCache;
    private readonly Mock<IEventBus> _eventBus;
    private readonly PatchExpenseCommandHandler _handler;

    public PatchExpenseCommandHandlerTests()
    {
        _cancellationToken = new();
        _validator = new();
        _mapper = new MapperConfiguration(c => c.AddProfile<ExpensesProfile>()).CreateMapper();
        _repository = new();
        _uow = new();
        _redisCache = new();
        _eventBus = new();

        _handler = new PatchExpenseCommandHandler(
            _validator.Object,
            _mapper,
            _repository.Object,
            _uow.Object,
            _redisCache.Object,
            _eventBus.Object
        );
    }

    [Theory]
    [MemberData(nameof(ExpensesMock.GetExpenses), MemberType = typeof(ExpensesMock))]
    public async Task PatchExpenseCommandHandler_ShouldPatchExpenseAndPublishEventAsync(
        IEnumerable<Expense> expenses
    )
    {
        // Arrange
        var request = new PatchExpenseCommand(
            expenses.First().Id,
            expenses.First().Name,
            expenses.First().Description,
            expenses.First().Value,
            expenses.First().CategoryId,
            expenses.First().ApplicationUserId
        );
        _validator
            .Setup(mock => mock.ValidateAsync(It.IsAny<PatchExpenseCommand>(), _cancellationToken))
            .ReturnsAsync(new ValidationResult())
            .Verifiable();
        _repository
            .Setup(mock => mock.PatchAsync(It.IsAny<Expense>(), _cancellationToken))
            .Verifiable();
        _uow.Setup(mock => mock.SaveChangesAsync(_cancellationToken)).Verifiable();
        _redisCache.Setup(mock => mock.RemoveKeysByPattern(It.IsAny<string>())).Verifiable();
        _eventBus
            .Setup(mock => mock.PublishAsync(It.IsAny<PatchedExpenseEvent>(), _cancellationToken))
            .Verifiable();

        // Act
        await _handler.Handle(request, _cancellationToken);

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
        _redisCache.Verify(mock => mock.RemoveKeysByPattern(It.IsAny<string>()), Times.Once);
        _eventBus.Verify(
            mock => mock.PublishAsync(It.IsAny<PatchedExpenseEvent>(), _cancellationToken),
            Times.Once
        );
    }
}
