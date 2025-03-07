namespace Infrastructure.Repositories;

public class BaseRepository<T, TEntity>(
    ApplicationDbContext context,
    ISpecificationEvaluator evaluator
) : IBaseRepository<T, TEntity>
    where T : IBaseEntity
    where TEntity : class, IBaseEntity
{
    private readonly ApplicationDbContext _context = context;
    private readonly ISpecificationEvaluator _evaluator = evaluator;

    public IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification) =>
        _evaluator.GetQuery(_context.Set<TEntity>().AsQueryable(), specification);

    public async Task<List<TEntity>> ListAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default
    ) => await ApplySpecification(specification).ToListAsync(cancellationToken);

    public async Task<TEntity?> GetBySpecAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default
    ) => await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);

    public async Task PostAsync(TEntity entity, CancellationToken cancellationToken) =>
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);

    public async Task PatchAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var foundEntity =
            await _context
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id.Equals(entity.Id), cancellationToken)
            ?? throw new NotFoundException(entity.Id);

        _context.Set<TEntity>().Entry(foundEntity).CurrentValues.SetValues(entity);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var foundEntity =
            await _context
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken)
            ?? throw new NotFoundException(id);

        _context.Set<TEntity>().Remove(foundEntity);
    }
}
