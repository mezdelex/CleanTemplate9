namespace Infrastructure.Repositories;

public class ExpensesRepository : BaseRepository<Expense>, IExpensesRepository
{
    public ExpensesRepository(ApplicationDbContext context, ISpecificationEvaluator evaluator)
        : base(context, evaluator) { }
}
