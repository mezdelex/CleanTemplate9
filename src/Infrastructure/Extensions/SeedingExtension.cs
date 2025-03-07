namespace Infrastructure.Extensions;

public static class SeedingExtension
{
    public static void SeedData(this ModelBuilder builder)
    {
        var categoryIds = new List<string>
        {
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
        };

        builder
            .Entity<Category>()
            .HasData(
                new()
                {
                    Id = categoryIds[0],
                    Name = "Groceries",
                    Description = "Groceries category.",
                },
                new()
                {
                    Id = categoryIds[1],
                    Name = "Transportation",
                    Description = "Transportation category.",
                },
                new()
                {
                    Id = categoryIds[2],
                    Name = "Leisure",
                    Description = "Leisure category.",
                },
                new()
                {
                    Id = categoryIds[3],
                    Name = "Utilities",
                    Description = "Utilities category.",
                }
            );
    }
}
