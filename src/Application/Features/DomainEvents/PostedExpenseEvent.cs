namespace Application.Features.DomainEvents;

public record PostedExpenseEvent(
    Guid Id,
    string Name,
    string Description,
    double Value,
    DateTime Date,
    Guid CategoryId
)
{
    public sealed class PostedExpenseEventConsumer : IConsumer<PostedExpenseEvent>
    {
        private readonly ILogger<PostedExpenseEventConsumer> _logger;

        public PostedExpenseEventConsumer(ILogger<PostedExpenseEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PostedExpenseEvent> context)
        {
            _logger.LogInformation("Expense posted: {@Expense}", context.Message);

            return Task.CompletedTask;
        }
    }
}
