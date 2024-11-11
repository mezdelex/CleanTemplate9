namespace Application.Repositories;

public interface IBaseRepository<T>
    where T : BaseEntity
{
    IQueryable<T> ApplySpecification(ISpecification<T> specification);
    Task<T?> GetBySpecAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    );
    Task<List<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    );
    Task PostAsync(T entity, CancellationToken cancellationToken = default);
    Task PatchAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
