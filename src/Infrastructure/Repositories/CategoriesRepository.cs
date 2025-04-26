namespace Infrastructure.Repositories;

public class CategoriesRepository(ApplicationDbContext context, ISpecificationEvaluator evaluator)
    : BaseRepository<Category>(context, evaluator),
        ICategoriesRepository { }
