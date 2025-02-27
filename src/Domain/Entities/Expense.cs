namespace Domain.Entities;

public class Expense : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime Date { get; set; }
    public Guid CategoryId { get; set; }

    public virtual Category Category { get; set; } = default!;
}

public static class ExpenseConstraints
{
    public const int NameMaxLength = 32;
    public const int DescriptionMaxLength = 256;
}
