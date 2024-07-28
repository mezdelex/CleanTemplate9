using Domain.Expenses;

namespace Application.Shared;

public record CategoryDTO(Guid id, string Name, string Description, List<Expense>? Expenses);
