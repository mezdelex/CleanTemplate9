namespace Application.Features.DomainEvents;

public sealed record PatchedCategoryEvent(string Id, string Name, string Description)
{
    public sealed class PatchedCategoryEventConsumer(ILogger<PatchedCategoryEventConsumer> logger)
        : IConsumer<PatchedCategoryEvent>
    {
        private readonly ILogger<PatchedCategoryEventConsumer> _logger = logger;

        public Task Consume(ConsumeContext<PatchedCategoryEvent> context)
        {
            _logger.LogInformation("Category patched: {@Category}", context.Message);

            return Task.CompletedTask;
        }
    }
}
