namespace Application.Features.DomainEvents;

public sealed record PostedCategoryEvent(Guid Id, string Name, string Description)
{
    public sealed class PostedCategoryEventConsumer : IConsumer<PostedCategoryEvent>
    {
        private readonly ILogger<PostedCategoryEventConsumer> _logger;

        public PostedCategoryEventConsumer(ILogger<PostedCategoryEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PostedCategoryEvent> context)
        {
            _logger.LogInformation("Category created: {@Category}", context.Message);

            return Task.CompletedTask;
        }
    }
}
