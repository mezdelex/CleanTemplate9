namespace Infrastructure.Repositories;

public class ExpensesRepository(ApplicationDbContext context, ISpecificationEvaluator evaluator)
    : BaseRepository<IBaseEntity, Expense>(context, evaluator),
        IExpensesRepository
{ }
