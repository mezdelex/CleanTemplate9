namespace Domain.Expenses;

public interface IExpensesRepository
{
    Task PatchAsync(Expense expense, CancellationToken cancellationToken);
    Task PostAsync(Expense expense, CancellationToken cancellationToken);
    Task DeleteAsync(Guid Id, CancellationToken cancellationToken);
}