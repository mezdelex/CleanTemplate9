namespace Domain.Categories;

public interface ICategoriesRepository
{
    Task PatchAsync(Category category, CancellationToken cancellation);
    Task PostAsync(Category category, CancellationToken cancellation);
    Task DeleteAsync(Guid Id, CancellationToken cancellation);
}
