namespace UnitTests.Features.Commands;

public sealed class PostExpenseCommandHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IValidator<PostExpenseCommand>> _validator;
    private readonly IMapper _mapper;
    private readonly Mock<IExpensesRepository> _repository;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IEventBus> _eventBus;
    private readonly PostExpenseCommandHandler _handler;

    public PostExpenseCommandHandlerTests()
    {
        _cancellationToken = new();
        _validator = new();
        _mapper = new MapperConfiguration(c => c.AddProfile<ExpensesProfile>()).CreateMapper();
        _repository = new();
        _uow = new();
        _eventBus = new();

        _handler = new PostExpenseCommandHandler(
            _validator.Object,
            _mapper,
            _repository.Object,
            _uow.Object,
            _eventBus.Object
        );
    }

    [Fact]
    public async Task PostExpenseCommandHandler_ShouldPostExpenseAndPublishEventAsync()
    {
        // Arrange
        var postExpenseCommand = new PostExpenseCommand(
            "Expense 1 name",
            "Expense 1 description",
            1,
            new Guid().ToString(),
            new Guid().ToString()
        );
        _validator
            .Setup(mock => mock.ValidateAsync(postExpenseCommand, _cancellationToken))
            .ReturnsAsync(new ValidationResult())
            .Verifiable();
        _repository
            .Setup(mock => mock.PostAsync(It.IsAny<Expense>(), _cancellationToken))
            .Verifiable();
        _uow.Setup(mock => mock.SaveChangesAsync(_cancellationToken)).Verifiable();
        _eventBus
            .Setup(mock => mock.PublishAsync(It.IsAny<PostedExpenseEvent>(), _cancellationToken))
            .Verifiable();

        // Act
        await _handler.Handle(postExpenseCommand, _cancellationToken);

        // Assert
        _validator.Verify(
            mock => mock.ValidateAsync(It.IsAny<PostExpenseCommand>(), _cancellationToken),
            Times.Once
        );
        _repository.Verify(
            mock => mock.PostAsync(It.IsAny<Expense>(), _cancellationToken),
            Times.Once
        );
        _uow.Verify(mock => mock.SaveChangesAsync(_cancellationToken), Times.Once);
        _eventBus.Verify(
            mock => mock.PublishAsync(It.IsAny<PostedExpenseEvent>(), _cancellationToken),
            Times.Once
        );
    }
}
