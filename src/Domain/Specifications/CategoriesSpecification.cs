namespace Domain.Specifications;

public sealed class CategoriesSpecification : Specification<Category>
{
    public CategoriesSpecification(
        string? id = null,
        string? name = null,
        string? containedWord = null
    )
    {
        Query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(id))
            Query.Where(x => x.Id.Equals(id));

        if (!string.IsNullOrWhiteSpace(name))
            Query.Where(x => x.Name.Equals(name));

        if (!string.IsNullOrWhiteSpace(containedWord))
            Query.Where(x =>
                x.Name.Contains(containedWord) || x.Description.Contains(containedWord)
            );

        Query.Include(x => x.Expenses).OrderBy(x => x.Name).ThenBy(x => x.Id);
    }
}
