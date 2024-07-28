namespace Domain.Expenses;

public interface IExpensesRepository
{
    Task PatchAsync(Guid Id, Expense expense, CancellationToken cancellation);
    Task PostAsync(Expense expense, CancellationToken cancellation);
    Task DeleteAsync(Guid Id, CancellationToken cancellation);
}
