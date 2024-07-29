using Application.Abstractions;
using Domain.Expenses;
using Domain.Persistence;
using FluentValidation;
using MediatR;

namespace Application.Expenses.PatchAsync;

public sealed class PatchExpenseCommandHandler : IRequestHandler<PatchExpenseCommand>
{
    private readonly IValidator<PatchExpenseCommand> _validator;
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IEventBus _eventBus;

    public PatchExpenseCommandHandler(
        IValidator<PatchExpenseCommand> validator,
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

    public async Task Handle(PatchExpenseCommand request, CancellationToken cancellationToken)
    {
        var results = await _validator.ValidateAsync(request, cancellationToken);
        if (!results.IsValid)
            throw new ValidationException(results.ToString().Replace("\r\n", " "));

        var expenseToPatch = new Expense(
            request.Id,
            request.Name,
            request.Description,
            request.Value,
            request.CategoryId
        );

        await _repository.PatchAsync(expenseToPatch, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        await _eventBus.PublishAsync(
            new PatchedExpenseEvent(
                expenseToPatch.Id,
                expenseToPatch.Name,
                expenseToPatch.Description,
                expenseToPatch.Value,
                expenseToPatch.CategoryId
            ),
            cancellationToken
        );
    }
}
