namespace Domain.Specifications;

public sealed class ExpensesSpecification : Specification<Expense>
{
    public ExpensesSpecification(
        Guid? id = null,
        string? name = null,
        string? containedWord = null,
        DateTime? minDate = null,
        DateTime? maxDate = null,
        Guid? categoryId = null
    )
    {
        Query.AsNoTracking();

        if (id != null)
            Query.Where(x => x.Id.Equals(id));

        if (!string.IsNullOrWhiteSpace(name))
            Query.Where(x => x.Name.Equals(name));

        if (!string.IsNullOrWhiteSpace(containedWord))
            Query.Where(x =>
                x.Name.Contains(containedWord) || x.Description.Contains(containedWord)
            );

        if (minDate.HasValue)
            Query.Where(x =>
                x.Date.CompareTo(DateTimeConversors.NormalizeToUtc(minDate.Value)) >= 0
            );

        if (maxDate.HasValue)
            Query.Where(x =>
                x.Date.CompareTo(DateTimeConversors.NormalizeToUtc(maxDate.Value)) <= 0
            );

        if (categoryId != null)
            Query.Where(x => x.CategoryId.Equals(categoryId));

        Query.OrderBy(x => x.CategoryId).ThenBy(x => x.Name);
    }
}
