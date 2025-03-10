namespace Application.Features.Shared;

public sealed record CategoryDTO(
    string Id,
    string Name,
    string Description,
    List<ExpenseDTO> Expenses
);
