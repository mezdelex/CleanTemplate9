namespace Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public virtual List<Expense> Expenses { get; set; } = default!;
}

public static class CategoryConstraints
{
    public const int NameMaxLength = 32;
    public const int DescriptionMaxLength = 256;
}
