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

    public Task PatchAsync(Guid Id, Expense expense, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public Task PostAsync(Expense expense, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid Id, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}