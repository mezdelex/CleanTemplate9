using Domain.Expenses;

namespace Application.Expenses.Shared;

public record CategoryDTO(Guid Id, string Name, string Description, List<Expense>? Expenses);