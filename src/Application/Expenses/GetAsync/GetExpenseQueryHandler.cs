using Application.Contexts;
using Application.Shared;
using Domain.Expenses;
using MediatR;

namespace Application.Expenses.GetAsync;

public sealed class GetExpenseQueryHandler : IRequestHandler<GetExpenseQuery, ExpenseDTO>
{
    private readonly IApplicationDbContext _context;

    public GetExpenseQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExpenseDTO> Handle(
        GetExpenseQuery request,
        CancellationToken cancellationToken
    )
    {
        var expense =
            await _context.Expenses.FindAsync(request.Id, cancellationToken)
            ?? throw new ExpenseNotFoundException(request.Id);

        return new ExpenseDTO(
            expense.Id,
            expense.Name,
            expense.Description,
            expense.Value,
            expense.CategoryId
        );
    }
}
