namespace Application.Expenses.PostAsync;

public record PostedExpenseEvent(
    Guid Id,
    string Name,
    string Description,
    double Value,
    Guid CategoryId
);
