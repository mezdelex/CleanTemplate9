namespace Infrastructure.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options),
        IApplicationDbContext
{
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Expense> Expenses { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        builder.SeedData();

        base.OnModelCreating(builder);
    }
}
