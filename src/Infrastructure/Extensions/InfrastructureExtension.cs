namespace Infrastructure.Extensions;

public static class InfrastructureExtension
{
    public static void AddInfrastructureDependencies(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                $"Server=sqlserver;Database={configuration["DATABASE"]};User Id=sa;Password={configuration["PASSWORD"]};TrustServerCertificate=True"
            )
        );
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>()
        );

        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            return ConnectionMultiplexer.Connect($"redis,password={configuration["PASSWORD"]}");
        });
        services.AddScoped<IDatabase>(provider =>
            provider.GetRequiredService<IConnectionMultiplexer>().GetDatabase()
        );
        services.AddScoped<IRedisCache, RedisCache>();
        services.AddScoped<IEventBus, RabbitMQEventBus>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ISpecificationEvaluator>(provider => new SpecificationEvaluator(true));
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<IExpensesRepository, ExpensesRepository>();

        services.AddMassTransit(busRegistrationConfigurator =>
        {
            busRegistrationConfigurator.SetKebabCaseEndpointNameFormatter();
            busRegistrationConfigurator.AddConsumer<PatchedCategoryEventConsumer>();
            busRegistrationConfigurator.AddConsumer<PatchedExpenseEventConsumer>();
            busRegistrationConfigurator.AddConsumer<PostedCategoryEventConsumer>();
            busRegistrationConfigurator.AddConsumer<PostedExpenseEventConsumer>();
            busRegistrationConfigurator.UsingRabbitMq(
                (busRegistrationContext, rabbitMQBusFactoryConfigurator) =>
                {
                    rabbitMQBusFactoryConfigurator.Host(
                        new Uri("amqp://rabbitmq"),
                        rabbitMQHostConfigurator =>
                        {
                            rabbitMQHostConfigurator.Username(configuration["USERNAME"]!);
                            rabbitMQHostConfigurator.Password(configuration["PASSWORD"]!);
                        }
                    );
                    rabbitMQBusFactoryConfigurator.ConfigureEndpoints(busRegistrationContext);
                }
            );
        });
    }
}
