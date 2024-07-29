using Application.Contexts;
using Application.Shared;
using MediatR;
using static Domain.Extensions.Collections.Collections;

namespace Application.Expenses.GetAllAsync;

public sealed class GetAllExpensesQueryHandler
    : IRequestHandler<GetAllExpensesQuery, PagedList<ExpenseDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllExpensesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<ExpenseDTO>> Handle(
        GetAllExpensesQuery request,
        CancellationToken cancellationToken
    ) =>
        await _context
            .Expenses.OrderBy(e => e.Name)
            .Select(e => new ExpenseDTO(e.Id, e.Name, e.Description, e.Value, e.CategoryId))
            .ToPagedListAsync(request.Page, request.PageSize, cancellationToken);
}
