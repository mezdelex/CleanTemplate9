using Domain.Expenses;

namespace Domain.Categories;

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

    public List<Expense>? Expenses { get; set; } = default;
}
