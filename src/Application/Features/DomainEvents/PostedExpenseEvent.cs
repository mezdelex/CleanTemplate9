namespace Application.Features.DomainEvents;

public record PostedExpenseEvent(
    string Id,
    string Name,
    string Description,
    double Value,
    DateTime Date,
    string CategoryId,
    string ApplicationUserId
)
{
    public sealed class PostedExpenseEventConsumer(ILogger<PostedExpenseEventConsumer> logger)
        : IConsumer<PostedExpenseEvent>
    {
        private readonly ILogger<PostedExpenseEventConsumer> _logger = logger;

        public Task Consume(ConsumeContext<PostedExpenseEvent> context)
        {
            _logger.LogInformation("Expense posted: {@Expense}", context.Message);

            return Task.CompletedTask;
        }
    }
}
