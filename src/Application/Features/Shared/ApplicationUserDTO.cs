namespace Application.Features.Shared;

public sealed record ApplicationUserDTO(
    string Id,
    string UserName,
    string Email,
    List<ExpenseDTO> Expenses
);
