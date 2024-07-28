using Domain.Categories;

namespace Domain.Expenses;

public class Expense
{
    public Expense(Guid id, string name, string description, double value, Guid categoryId)
    {
        Id = id;
        Name = name;
        Description = description;
        Value = value;
        CategoryId = categoryId;
    }

    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Value { get; set; }
    public Guid CategoryId { get; set; }

    public Category? Category { get; set; } = default;
}