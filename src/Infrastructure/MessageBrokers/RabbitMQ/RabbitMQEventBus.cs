using Application.Abstractions;
using MassTransit;

namespace Infrastructure.MessageBrokers.RabbitMQ;

public sealed class RabbitMQEventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public RabbitMQEventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class => _publishEndpoint.Publish(message, cancellationToken);
}
