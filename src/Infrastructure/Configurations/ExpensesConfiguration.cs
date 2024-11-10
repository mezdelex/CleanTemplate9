namespace Infrastructure.Configurations;

public class ExpensesConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(30).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(256);
        builder.Property(e => e.Value).IsRequired();

        builder
            .HasOne(e => e.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
