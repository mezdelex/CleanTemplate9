using MediatR;

namespace Application.Expenses.PatchAsync;

public record PatchExpenseCommand(
    Guid Id,
    string Name,
    string Description,
    double Value,
    Guid CategoryId
) : IRequest;
