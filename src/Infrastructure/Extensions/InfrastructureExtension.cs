using Application.Abstractions;
using Application.Categories.PatchAsync;
using Application.Categories.PostAsync;
using Application.Contexts;
using Application.Expenses.PatchAsync;
using Application.Expenses.PostAsync;
using Domain.Categories;
using Domain.Expenses;
using Domain.Persistence;
using Infrastructure.Contexts;
using Infrastructure.MessageBrokers.RabbitMQ;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Extensions;

public static class InfrastructureExtension
{
    public static void AddInfrastructureDependencies(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>()
        );

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        services.AddScoped<IExpensesRepository, ExpensesRepository>();

        services.Configure<RabbitMQSettings>(rabbitMQSettings =>
        {
            var rabbitMQSection = configuration.GetSection("RabbitMQ");

            rabbitMQSettings.Host = rabbitMQSection["Host"] ?? string.Empty;
            rabbitMQSettings.Username = rabbitMQSection["Username"] ?? string.Empty;
            rabbitMQSettings.Password = rabbitMQSection["Password"] ?? string.Empty;
        });
        services.AddSingleton(provider =>
            provider.GetRequiredService<IOptions<RabbitMQSettings>>().Value
        );
        services.AddScoped<IEventBus, RabbitMQEventBus>();

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
                    var settings = busRegistrationContext.GetRequiredService<RabbitMQSettings>();

                    rabbitMQBusFactoryConfigurator.Host(
                        new Uri(settings.Host),
                        rabbitMQHostConfigurator =>
                        {
                            rabbitMQHostConfigurator.Username(settings.Username);
                            rabbitMQHostConfigurator.Password(settings.Password);
                        }
                    );
                    rabbitMQBusFactoryConfigurator.ConfigureEndpoints(busRegistrationContext);
                }
            );
        });
    }
}
