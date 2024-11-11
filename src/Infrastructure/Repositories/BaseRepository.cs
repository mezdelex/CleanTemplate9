namespace Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T>
    where T : BaseEntity
{
    private readonly ApplicationDbContext _context;
    private readonly ISpecificationEvaluator _evaluator;

    public BaseRepository(ApplicationDbContext context, ISpecificationEvaluator evaluator)
    {
        _context = context;
        _evaluator = evaluator;
    }

    public IQueryable<T> ApplySpecification(ISpecification<T> specification) =>
        _evaluator.GetQuery(_context.Set<T>().AsQueryable(), specification);

    public async Task<List<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    ) => await ApplySpecification(specification).ToListAsync(cancellationToken);

    public async Task<T?> GetBySpecAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    ) => await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);

    public async Task PostAsync(T entity, CancellationToken cancellationToken) =>
        await _context.Set<T>().AddAsync(entity, cancellationToken);

    public async Task PatchAsync(T entity, CancellationToken cancellationToken)
    {
        var foundEntity =
            await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken)
            ?? throw new NotFoundException(entity.Id);

        _context.Set<T>().Entry(foundEntity).CurrentValues.SetValues(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity =
            await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new NotFoundException(id);

        _context.Set<T>().Remove(foundEntity);
    }
}
