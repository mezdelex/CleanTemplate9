namespace Domain.Entities;

public class Category : IBaseEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public virtual List<Expense> Expenses { get; set; } = default!;
}

public static class CategoryConstraints
{
    public const int IdMaxLength = 36;
    public const int NameMaxLength = 32;
    public const int DescriptionMaxLength = 256;
}
