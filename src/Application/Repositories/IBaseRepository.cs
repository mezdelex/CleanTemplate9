namespace Application.Repositories;

public interface IBaseRepository<T, TEntity>
    where T : IBaseEntity
    where TEntity : class, IBaseEntity
{
    IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification);
    Task<TEntity?> GetBySpecAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default
    );
    Task<List<TEntity>> ListAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default
    );
    Task PostAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task PatchAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}
