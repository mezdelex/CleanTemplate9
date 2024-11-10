namespace Application.Repositories;

public interface ICategoriesRepository
{
    Task PostAsync(Category category, CancellationToken cancellationToken);
    Task PatchAsync(Category category, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
