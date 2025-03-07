namespace Domain.Identity;

public class ApplicationUser : IdentityUser, IBaseEntity
{
    public virtual IList<Expense> Expenses { get; set; } = default!;
}

public static class ApplicationUserConstraints
{
    public const int IdMaxLength = 36;
}
