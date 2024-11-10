namespace Domain.Entities;

public class Category
{
    public Category(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual List<Expense> Expenses { get; set; } = default!;
}
