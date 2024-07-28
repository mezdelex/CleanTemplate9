using Infrastructure.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        app.ApplicationServices.CreateScope()
            .ServiceProvider.GetRequiredService<ApplicationDbContext>()
            .Database.Migrate();
    }
}
