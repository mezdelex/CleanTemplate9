using Application.Shared;
using MediatR;
using static Domain.Extensions.Collections.Collections;

namespace Application.Expenses.GetAllAsync;

public record GetAllExpensesQuery(int Page, int PageSize) : IRequest<PagedList<ExpenseDTO>>;
