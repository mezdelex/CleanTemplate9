using MediatR;

namespace Application.Expenses.DeleteAsync;

public record DeleteExpenseCommand(Guid Id) : IRequest;
