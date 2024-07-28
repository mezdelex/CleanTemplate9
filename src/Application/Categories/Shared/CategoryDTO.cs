using Domain.Expenses;

namespace Application.Categories.Shared;

public record CategoryDTO(Guid Id, string Name, string Description, List<Expense>? Expenses);
