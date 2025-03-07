namespace Application.Features.Commands;

public record DeleteExpenseCommand(string Id) : IRequest
{
    public sealed class DeleteExpenseCommandHandler(IExpensesRepository repository, IUnitOfWork uow)
        : IRequestHandler<DeleteExpenseCommand>
    {
        private readonly IExpensesRepository _repository = repository;
        private readonly IUnitOfWork _uow = uow;

        public async Task Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
    {
        public DeleteExpenseCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(Id)))
                .MaximumLength(ExpenseConstraints.IdMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(Id),
                        ExpenseConstraints.IdMaxLength
                    )
                );
        }
    }
}
