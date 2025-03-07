namespace Application.Features.Shared;

public sealed record ExpenseDTO(
    string Id,
    string Name,
    string Description,
    double Value,
    DateTime Date,
    string CategoryId,
    string ApplicationUserId
);
