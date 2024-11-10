namespace Application.Features.DomainEvents;

public sealed record PatchedCategoryEvent(Guid Id, string Name, string Description)
{
    public sealed class PatchedCategoryEventConsumer : IConsumer<PatchedCategoryEvent>
    {
        private readonly ILogger<PatchedCategoryEventConsumer> _logger;

        public PatchedCategoryEventConsumer(ILogger<PatchedCategoryEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PatchedCategoryEvent> context)
        {
            _logger.LogInformation("Category patched: {@Category}", context.Message);

            return Task.CompletedTask;
        }
    }
}
