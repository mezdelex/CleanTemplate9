namespace Domain.Specifications;

public sealed class ApplicationUsersSpecification : Specification<ApplicationUser>
{
    public ApplicationUsersSpecification(
        string? id = null,
        string? email = null,
        string? containedWord = null
    )
    {
        Query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(id))
        {
            Query.Where(x => x.Id.Equals(id));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            Query.Where(x => x.Email!.Equals(email));
        }

        if (!string.IsNullOrWhiteSpace(containedWord))
        {
            Query.Where(x =>
                x.Email!.Contains(containedWord) || x.UserName!.Contains(containedWord)
            );
        }

        Query.Include(x => x.Expenses).OrderBy(x => x.Email).ThenBy(x => x.Id);
    }
}
