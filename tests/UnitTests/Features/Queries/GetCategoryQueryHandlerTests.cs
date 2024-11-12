namespace Application.UnitTests.Categories.GetAsync;

public sealed class GetCategoryQueryHandlerTests
{
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<ICategoriesRepository> _repository;
    private readonly GetCategoryQueryHandler _handler;

    public GetCategoryQueryHandlerTests()
    {
        _cancellationToken = new();
        _repository = new();

        _handler = new GetCategoryQueryHandler(_repository.Object);
    }

    [Fact]
    public async Task Handle_ValidIdGetCategoryQuery_ShouldReturnRequestedCategoryAsCategoryDTOAsync()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var getCategoryQuery = new GetCategoryQuery(guid);
        var category = new Category
        {
            Id = guid,
            Name = "Name 1",
            Description = "Description 1",
        };
        _repository
            .Setup(mock =>
                mock.GetBySpecAsync(It.IsAny<CategoriesSpecification>(), _cancellationToken)
            )
            .ReturnsAsync(category)
            .Verifiable();

        // Act
        var result = await _handler.Handle(getCategoryQuery, _cancellationToken);

        // Assert
        result.Id.Should().Be(category.Id);
        result.Name.Should().Be(category.Name);
        result.Description.Should().Be(category.Description);
        _repository.Verify(
            mock => mock.GetBySpecAsync(It.IsAny<CategoriesSpecification>(), _cancellationToken),
            Times.Once
        );
    }
}
