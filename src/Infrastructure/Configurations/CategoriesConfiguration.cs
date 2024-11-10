namespace Infrastructure.Configurations;

public class CategoriesConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(30).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(256);
    }
}
