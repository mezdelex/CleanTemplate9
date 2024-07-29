using MediatR;

namespace Application.Expenses.PostAsync;

public record PostExpenseCommand(string Name, string Description, double Value, Guid CategoryId)
    : IRequest;
