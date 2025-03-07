namespace Application.Features.DomainEvents;

public sealed record PostedCategoryEvent(string Id, string Name, string Description)
{
    public sealed class PostedCategoryEventConsumer(ILogger<PostedCategoryEventConsumer> logger)
        : IConsumer<PostedCategoryEvent>
    {
        private readonly ILogger<PostedCategoryEventConsumer> _logger = logger;

        public Task Consume(ConsumeContext<PostedCategoryEvent> context)
        {
            _logger.LogInformation("Category created: {@Category}", context.Message);

            return Task.CompletedTask;
        }
    }
}
