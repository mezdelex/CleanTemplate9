namespace Application.Shared;

public record ExpenseDTO(Guid id, string Name, string Description, double Value, Guid CategoryId);
