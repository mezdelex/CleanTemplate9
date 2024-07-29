using Application.Contexts;
using Domain.Expenses;

namespace Infrastructure.Repositories;

public class ExpensesRepository : IExpensesRepository
{
    private readonly IApplicationDbContext _context;

    public ExpensesRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task PatchAsync(Expense expense, CancellationToken cancellationToken)
    {
        var foundExpense =
            await _context.Expenses.FindAsync(expense.Id, cancellationToken)
            ?? throw new ExpenseNotFoundException(expense.Id);

        _context.Expenses.Entry(foundExpense).CurrentValues.SetValues(expense);
        _context.Expenses.Entry(foundExpense).Property(p => p.Id).IsModified = false;
    }

    public async Task PostAsync(Expense expense, CancellationToken cancellationToken) =>
        await _context.Expenses.AddAsync(expense, cancellationToken);

    public async Task DeleteAsync(Guid id, CancellationToken cancellation)
    {
        var foundExpense =
            await _context.Expenses.FindAsync(id, cancellation)
            ?? throw new ExpenseNotFoundException(id);

        _context.Expenses.Remove(foundExpense);
    }
}