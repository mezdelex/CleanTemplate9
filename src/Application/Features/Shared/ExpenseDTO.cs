namespace Application.Features.Shared;

public record ExpenseDTO(Guid Id, string Name, string Description, double Value, Guid CategoryId);
