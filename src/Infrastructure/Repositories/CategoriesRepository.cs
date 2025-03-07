namespace Infrastructure.Repositories;

public class CategoriesRepository(ApplicationDbContext context, ISpecificationEvaluator evaluator)
    : BaseRepository<IBaseEntity, Category>(context, evaluator),
        ICategoriesRepository
{ }
