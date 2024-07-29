using Application.Shared;
using MediatR;

namespace Application.Expenses.GetAsync;

public record GetExpenseQuery(Guid Id) : IRequest<ExpenseDTO>;
