namespace Domain.Specifications;

public sealed class ExpensesSpecification : Specification<Expense>
{
    public ExpensesSpecification(
        Guid? id = null,
        string? name = null,
        string? containedWord = null,
        Guid? categoryId = null
    )
    {
        Query.AsNoTracking();

        if (id != null)
            Query.Where(x => x.Id.Equals(id));

        if (!string.IsNullOrWhiteSpace(name))
            Query.Where(x => x.Name.Equals(name));

        if (categoryId != null)
            Query.Where(x => x.CategoryId.Equals(categoryId));

        if (!string.IsNullOrWhiteSpace(containedWord))
            Query.Where(x =>
                x.Name.Contains(containedWord) || x.Description.Contains(containedWord)
            );

        Query.OrderBy(x => x.CategoryId).ThenBy(x => x.Name);
    }
}
