namespace Domain.Specifications;

public sealed class ExpensesSpecification : Specification<Expense>
{
    public ExpensesSpecification(
        string? id = null,
        string? name = null,
        string? containedWord = null,
        DateTime? minDate = null,
        DateTime? maxDate = null,
        string? categoryId = null,
        string? applicationUserId = null
    )
    {
        Query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(id))
        {
            Query.Where(x => x.Id.Equals(id));
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            Query.Where(x => x.Name.Equals(name));
        }

        if (!string.IsNullOrWhiteSpace(containedWord))
        {
            Query.Where(x =>
                x.Name.Contains(containedWord) || x.Description.Contains(containedWord)
            );
        }

        /*TODO: add TimeZoneOffset by passing the TimeZone in the request (recommended method)*/
        if (minDate.HasValue)
        {
            Query.Where(x =>
                x.Date.CompareTo(DateTimeConversors.NormalizeToUtc(minDate.Value)) >= 0
            );
        }

        if (maxDate.HasValue)
        {
            Query.Where(x =>
                x.Date.CompareTo(DateTimeConversors.NormalizeToUtc(maxDate.Value)) <= 0
            );
        }

        if (!string.IsNullOrWhiteSpace(categoryId))
        {
            Query.Where(x => x.CategoryId.Equals(categoryId));
        }

        if (!string.IsNullOrWhiteSpace(applicationUserId))
        {
            Query.Where(x => x.ApplicationUserId.Equals(applicationUserId));
        }

        Query.OrderBy(x => x.CategoryId).ThenBy(x => x.Name).ThenBy(x => x.Id);
    }
}
