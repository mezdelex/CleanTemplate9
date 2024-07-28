namespace Infrastructure.MessageBrokers.RabbitMQ;

public sealed class RabbitMQSettings
{
    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}