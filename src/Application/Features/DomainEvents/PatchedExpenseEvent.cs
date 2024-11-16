namespace Application.Features.DomainEvents;

public sealed record PatchedExpenseEvent(
    Guid Id,
    string Name,
    string Description,
    double Value,
    DateTime Date,
    Guid CategoryId
)
{
    public sealed class PatchedExpenseEventConsumer : IConsumer<PatchedExpenseEvent>
    {
        private readonly ILogger<PatchedExpenseEventConsumer> _logger;

        public PatchedExpenseEventConsumer(ILogger<PatchedExpenseEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PatchedExpenseEvent> context)
        {
            _logger.LogInformation("Expense patched: {@Expense}", context.Message);

            return Task.CompletedTask;
        }
    }
}
