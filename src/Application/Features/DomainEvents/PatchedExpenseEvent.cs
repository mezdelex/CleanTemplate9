namespace Application.Features.DomainEvents;

public sealed record PatchedExpenseEvent(
    string Id,
    string Name,
    string Description,
    double Value,
    DateTime Date,
    string CategoryId,
    string ApplicationUserId
)
{
    public sealed class PatchedExpenseEventConsumer(ILogger<PatchedExpenseEventConsumer> logger)
        : IConsumer<PatchedExpenseEvent>
    {
        private readonly ILogger<PatchedExpenseEventConsumer> _logger = logger;

        public Task Consume(ConsumeContext<PatchedExpenseEvent> context)
        {
            _logger.LogInformation("Expense patched: {@Expense}", context.Message);

            return Task.CompletedTask;
        }
    }
}
