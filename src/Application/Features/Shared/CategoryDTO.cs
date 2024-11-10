namespace Application.Features.Shared;

public record CategoryDTO(Guid Id, string Name, string Description, List<Expense> Expenses);
