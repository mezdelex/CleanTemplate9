namespace UnitTests.Features.Commands;

public sealed class DeleteCategoryCommandHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<ICategoriesRepository> _repository;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly DeleteCategoryCommandHandler _handler;

    public DeleteCategoryCommandHandlerTests()
    {
        _cancellationToken = new();
        _repository = new();
        _uow = new();

        _handler = new DeleteCategoryCommandHandler(_repository.Object, _uow.Object);
    }

    [Theory]
    [MemberData(nameof(CategoriesMock.GetCategories), MemberType = typeof(CategoriesMock))]
    public async Task DeleteCategoryCommandHandler_ShouldDeleteCategory(
        IEnumerable<Category> categories
    )
    {
        // Arrange
        var deleteCategoryCommand = new DeleteCategoryCommand(categories.First().Id);
        _repository
            .Setup(mock => mock.DeleteAsync(It.IsAny<string>(), _cancellationToken))
            .Verifiable();
        _uow.Setup(mock => mock.SaveChangesAsync(_cancellationToken)).Verifiable();

        // Act
        await _handler.Handle(deleteCategoryCommand, _cancellationToken);

        // Assert
        _repository.Verify(
            mock => mock.DeleteAsync(It.IsAny<string>(), _cancellationToken),
            Times.Once
        );
        _uow.Verify(mock => mock.SaveChangesAsync(_cancellationToken), Times.Once);
    }
}
