namespace Infrastructure.Configurations;

public class ExpensesConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(ExpenseConstraints.NameMaxLength).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(ExpenseConstraints.DescriptionMaxLength);
        builder.Property(e => e.Value).IsRequired();

        builder
            .HasOne(e => e.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        builder
            .HasOne(e => e.ApplicationUser)
            .WithMany(au => au.Expenses)
            .HasForeignKey(e => e.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
