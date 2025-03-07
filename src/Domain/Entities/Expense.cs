namespace Domain.Entities;

public class Expense : IBaseEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime Date { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public string ApplicationUserId { get; set; } = string.Empty;

    public virtual ApplicationUser ApplicationUser { get; set; } = default!;
    public virtual Category Category { get; set; } = default!;
}

public static class ExpenseConstraints
{
    public const int IdMaxLength = 36;
    public const int NameMaxLength = 32;
    public const int DescriptionMaxLength = 256;
    public const int CategoryIdMaxLength = 36;
    public const int ApplicationUserIdMaxLength = 36;
}
