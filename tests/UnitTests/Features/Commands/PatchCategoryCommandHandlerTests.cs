namespace UnitTests.Features.Commands;

public sealed class PatchCategoryCommandHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IValidator<PatchCategoryCommand>> _validator;
    private readonly IMapper _mapper;
    private readonly Mock<ICategoriesRepository> _repository;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IRedisCache> _redisCache;
    private readonly Mock<IEventBus> _eventBus;
    private readonly PatchCategoryCommandHandler _handler;

    public PatchCategoryCommandHandlerTests()
    {
        _cancellationToken = new();
        _validator = new();
        _mapper = new MapperConfiguration(c => c.AddProfile<CategoriesProfile>()).CreateMapper();
        _repository = new();
        _uow = new();
        _redisCache = new();
        _eventBus = new();

        _handler = new PatchCategoryCommandHandler(
            _validator.Object,
            _mapper,
            _repository.Object,
            _uow.Object,
            _redisCache.Object,
            _eventBus.Object
        );
    }

    [Theory]
    [MemberData(nameof(CategoriesMock.GetCategories), MemberType = typeof(CategoriesMock))]
    public async Task PatchCategoryCommandHandler_ShouldPatchCategoryAndPublishEventAsync(
        IEnumerable<Category> categories
    )
    {
        // Arrange
        var request = new PatchCategoryCommand(
            categories.First().Id,
            categories.First().Name,
            categories.First().Description
        );
        _validator
            .Setup(mock => mock.ValidateAsync(It.IsAny<PatchCategoryCommand>(), _cancellationToken))
            .ReturnsAsync(new ValidationResult())
            .Verifiable();
        _repository
            .Setup(mock => mock.PatchAsync(It.IsAny<Category>(), _cancellationToken))
            .Verifiable();
        _uow.Setup(mock => mock.SaveChangesAsync(_cancellationToken)).Verifiable();
        _redisCache.Setup(mock => mock.RemoveKeysByPattern(It.IsAny<string>())).Verifiable();
        _eventBus
            .Setup(mock => mock.PublishAsync(It.IsAny<PatchedCategoryEvent>(), _cancellationToken))
            .Verifiable();

        // Act
        await _handler.Handle(request, _cancellationToken);

        // Assert
        _validator.Verify(
            mock => mock.ValidateAsync(It.IsAny<PatchCategoryCommand>(), _cancellationToken),
            Times.Once
        );
        _repository.Verify(
            mock => mock.PatchAsync(It.IsAny<Category>(), _cancellationToken),
            Times.Once
        );
        _uow.Verify(mock => mock.SaveChangesAsync(_cancellationToken), Times.Once);
        _redisCache.Verify(mock => mock.RemoveKeysByPattern(It.IsAny<string>()), Times.Once());
        _eventBus.Verify(
            mock => mock.PublishAsync(It.IsAny<PatchedCategoryEvent>(), _cancellationToken),
            Times.Once
        );
    }
}
