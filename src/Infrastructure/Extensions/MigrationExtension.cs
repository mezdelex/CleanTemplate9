namespace Infrastructure.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        app.ApplicationServices.CreateScope()
            .ServiceProvider.GetRequiredService<ApplicationDbContext>()
            .Database.MigrateAsync();
    }
}
