namespace Infrastructure.Extensions;

public static class SeedingExtension
{
    public static void SeedData(this ModelBuilder builder)
    {
        var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        builder
            .Entity<Category>()
            .HasData(
                new()
                {
                    Id = ids[0],
                    Name = "Groceries",
                    Description = "Groceries category.",
                },
                new()
                {
                    Id = ids[1],
                    Name = "Transportation",
                    Description = "Transportation category.",
                },
                new()
                {
                    Id = ids[2],
                    Name = "Leisure",
                    Description = "Leisure category.",
                },
                new()
                {
                    Id = ids[3],
                    Name = "Utilities",
                    Description = "Utilities category.",
                }
            );

        builder
            .Entity<Expense>()
            .HasData(
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "200g of chicken",
                    Description = "Some chicken bought in the supermarket",
                    Value = 2.05,
                    Date = DateTime.UtcNow,
                    CategoryId = ids[0],
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "1 can of beans",
                    Description = "A can of beans bought in the supermarket",
                    Value = 2,
                    Date = DateTime.UtcNow,
                    CategoryId = ids[0],
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Full car tank",
                    Description = "Full car tank of 95",
                    Value = 50,
                    Date = DateTime.UtcNow,
                    CategoryId = ids[1],
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Random film",
                    Description = "A film I watched",
                    Value = 5.90,
                    Date = DateTime.UtcNow,
                    CategoryId = ids[2],
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Electricity",
                    Description = "Electricity bill",
                    Value = 40,
                    Date = DateTime.UtcNow,
                    CategoryId = ids[3],
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Water",
                    Description = "Water bill",
                    Value = 15,
                    Date = DateTime.UtcNow,
                    CategoryId = ids[3],
                }
            );
    }
}
