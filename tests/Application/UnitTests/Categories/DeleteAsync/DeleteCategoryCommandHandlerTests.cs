using Application.Categories.DeleteAsync;
using Domain.Categories;
using Domain.Persistence;
using Moq;

namespace Application.UnitTests.Categories.PostAsync;

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

    [Fact]
    public async Task DeleteCategoryCommandHandler_ShouldDeleteCategory()
    {
        // Arrange
        var deleteCategoryCommand = new DeleteCategoryCommand(Guid.NewGuid());
        _repository
            .Setup(mock => mock.DeleteAsync(It.IsAny<Guid>(), _cancellationToken))
            .Verifiable();
        _uow.Setup(mock => mock.SaveChangesAsync(_cancellationToken)).Verifiable();

        // Act
        await _handler.Handle(deleteCategoryCommand, _cancellationToken);

        // Assert
        _repository.Verify();
        _uow.Verify();
    }
}
