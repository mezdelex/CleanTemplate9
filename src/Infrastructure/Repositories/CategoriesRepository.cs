namespace Infrastructure.Repositories;

public class CategoriesRepository : BaseRepository<Category>, ICategoriesRepository
{
    public CategoriesRepository(ApplicationDbContext context, ISpecificationEvaluator evaluator)
        : base(context, evaluator) { }
}
