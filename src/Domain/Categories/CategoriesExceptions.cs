namespace Domain.Categories;

public sealed class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException(Guid id)
        : base($"The category with id {id} could not be found.") { }
}
