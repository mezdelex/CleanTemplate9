namespace Infrastructure.Configurations;

public class CategoriesConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(CategoryConstraints.NameMaxLength).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(CategoryConstraints.DescriptionMaxLength);
    }
}
