namespace Application.UnitTests.Categories.PostAsync;

public sealed class PatchCategoryCommandHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IValidator<PatchCategoryCommand>> _validator;
    private readonly Mock<ICategoriesRepository> _repository;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IEventBus> _eventBus;
    private readonly PatchCategoryCommandHandler _handler;

    public PatchCategoryCommandHandlerTests()
    {
        _cancellationToken = new();
        _validator = new();
        _repository = new();
        _uow = new();
        _eventBus = new();

        _handler = new PatchCategoryCommandHandler(
            _validator.Object,
            _repository.Object,
            _uow.Object,
            _eventBus.Object
        );
    }

    [Fact]
    public async Task PatchCategoryCommandHandler_ShouldPatchCategoryAndPublishEventAsync()
    {
        // Arrange
        var patchCategoryCommand = new PatchCategoryCommand(
            Guid.NewGuid(),
            "Category 1 name",
            "Category 1 description"
        );
        _validator
            .Setup(mock => mock.ValidateAsync(patchCategoryCommand, _cancellationToken))
            .ReturnsAsync(new ValidationResult())
            .Verifiable();
        _repository
            .Setup(mock => mock.PatchAsync(It.IsAny<Category>(), _cancellationToken))
            .Verifiable();
        _uow.Setup(mock => mock.SaveChangesAsync(_cancellationToken)).Verifiable();
        _eventBus
            .Setup(mock => mock.PublishAsync(It.IsAny<PatchedCategoryEvent>(), _cancellationToken))
            .Verifiable();

        // Act
        await _handler.Handle(patchCategoryCommand, _cancellationToken);

        // Assert
        _validator.Verify();
        _repository.Verify();
        _uow.Verify();
        _eventBus.Verify();
    }
}
