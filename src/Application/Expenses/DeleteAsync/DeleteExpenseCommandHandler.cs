using Domain.Expenses;
using Domain.Persistence;
using MediatR;

namespace Application.Expenses.DeleteAsync;

public sealed class PostExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand>
{
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _uow;

    public PostExpenseCommandHandler(IExpensesRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
