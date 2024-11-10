namespace Application.Repositories;

public interface IExpensesRepository
{
    Task PostAsync(Expense expense, CancellationToken cancellationToken);
    Task PatchAsync(Expense expense, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
