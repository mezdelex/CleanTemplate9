namespace Application.Expenses.PatchAsync;

public record PatchedExpenseEvent(
    Guid Id,
    string Name,
    string Description,
    double Value,
    Guid CategoryId
);