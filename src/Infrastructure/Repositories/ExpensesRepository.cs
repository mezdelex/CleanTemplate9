namespace Infrastructure.Repositories;

public class ExpensesRepository(ApplicationDbContext context, ISpecificationEvaluator evaluator)
    : BaseRepository<Expense>(context, evaluator),
        IExpensesRepository { }
