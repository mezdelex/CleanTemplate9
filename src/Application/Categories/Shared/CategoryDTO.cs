using Domain.Expenses;

namespace Application.Categories.Shared;

public record CategoryDTO(Guid id, string Name, string Description, List<Expense>? Expenses);