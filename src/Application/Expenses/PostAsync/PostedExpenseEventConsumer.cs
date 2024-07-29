using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Expenses.PostAsync;

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