namespace UnitTests.Features.Commands;

public sealed class PostCategoryCommandHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IValidator<PostCategoryCommand>> _validator;
    private readonly IMapper _mapper;
    private readonly Mock<ICategoriesRepository> _repository;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IEventBus> _eventBus;
    private readonly PostCategoryCommandHandler _handler;

    public PostCategoryCommandHandlerTests()
    {
        _cancellationToken = new();
        _validator = new();
        _mapper = new MapperConfiguration(c => c.AddProfile<CategoriesProfile>()).CreateMapper();
        _repository = new();
        _uow = new();
        _eventBus = new();

        _handler = new PostCategoryCommandHandler(
            _validator.Object,
            _mapper,
            _repository.Object,
            _uow.Object,
            _eventBus.Object
        );
    }

    [Fact]
    public async Task PostCategoryCommandHandler_ShouldPostCategoryAndPublishEventAsync()
    {
        // Arrange
        var postCategoryCommand = new PostCategoryCommand(
            "Category 1 name",
            "Category 1 description"
        );
        _validator
            .Setup(mock => mock.ValidateAsync(postCategoryCommand, _cancellationToken))
            .ReturnsAsync(new ValidationResult())
            .Verifiable();
        _repository
            .Setup(mock => mock.PostAsync(It.IsAny<Category>(), _cancellationToken))
            .Verifiable();
        _uow.Setup(mock => mock.SaveChangesAsync(_cancellationToken)).Verifiable();
        _eventBus
            .Setup(mock => mock.PublishAsync(It.IsAny<PostedCategoryEvent>(), _cancellationToken))
            .Verifiable();

        // Act
        await _handler.Handle(postCategoryCommand, _cancellationToken);

        // Assert
        _validator.Verify(
            mock => mock.ValidateAsync(It.IsAny<PostCategoryCommand>(), _cancellationToken),
            Times.Once
        );
        _repository.Verify(
            mock => mock.PostAsync(It.IsAny<Category>(), _cancellationToken),
            Times.Once
        );
        _uow.Verify(mock => mock.SaveChangesAsync(_cancellationToken), Times.Once);
        _eventBus.Verify(
            mock => mock.PublishAsync(It.IsAny<PostedCategoryEvent>(), _cancellationToken),
            Times.Once
        );
    }
}
