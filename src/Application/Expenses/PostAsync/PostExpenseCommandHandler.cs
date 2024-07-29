using Application.Abstractions;
using Domain.Expenses;
using Domain.Persistence;
using FluentValidation;
using MediatR;

namespace Application.Expenses.PostAsync;

public sealed class PostExpenseCommandHandler : IRequestHandler<PostExpenseCommand>
{
    private readonly IValidator<PostExpenseCommand> _validator;
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IEventBus _eventBus;

    public PostExpenseCommandHandler(
        IValidator<PostExpenseCommand> validator,
        IExpensesRepository repository,
        IUnitOfWork uow,
        IEventBus eventBus
    )
    {
        _validator = validator;
        _repository = repository;
        _uow = uow;
        _eventBus = eventBus;
    }

    public async Task Handle(PostExpenseCommand request, CancellationToken cancellationToken)
    {
        var results = await _validator.ValidateAsync(request, cancellationToken);
        if (!results.IsValid)
            throw new ValidationException(results.ToString().Replace("\r\n", " "));

        var expenseToPost = new Expense(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.Value,
            request.CategoryId
        );

        await _repository.PostAsync(expenseToPost, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        await _eventBus.PublishAsync(
            new PostedExpenseEvent(
                expenseToPost.Id,
                expenseToPost.Name,
                expenseToPost.Description,
                expenseToPost.Value,
                expenseToPost.CategoryId
            ),
            cancellationToken
        );
    }
}