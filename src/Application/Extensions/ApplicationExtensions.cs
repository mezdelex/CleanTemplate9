namespace Application.Extensions;

public static class ApplicationExtension
{
    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationExtension).Assembly;

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(assembly);
    }
}
