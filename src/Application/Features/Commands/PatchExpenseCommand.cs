namespace Application.Features.Commands;

public sealed record PatchExpenseCommand(
    string Id,
    string Name,
    string Description,
    double Value,
    string CategoryId,
    string ApplicationUserId
) : IRequest
{
    public sealed class PatchExpenseCommandHandler(
        IValidator<PatchExpenseCommand> validator,
        IMapper mapper,
        IExpensesRepository repository,
        IUnitOfWork uow,
        IRedisCache redisCache,
        IEventBus eventBus
    ) : IRequestHandler<PatchExpenseCommand>
    {
        private readonly IValidator<PatchExpenseCommand> _validator = validator;
        private readonly IMapper _mapper = mapper;
        private readonly IExpensesRepository _repository = repository;
        private readonly IUnitOfWork _uow = uow;
        private readonly IRedisCache _redisCache = redisCache;
        private readonly IEventBus _eventBus = eventBus;

        public async Task Handle(PatchExpenseCommand request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
                throw new ValidationException(results.ToString().Replace("\r\n", " "));

            var expenseToPatch = _mapper.Map<Expense>(request);

            await _repository.PatchAsync(expenseToPatch, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            await _redisCache.RemoveKeysByPattern(nameof(Expense));
            await _eventBus.PublishAsync(
                _mapper.Map<PatchedExpenseEvent>(expenseToPatch),
                cancellationToken
            );
        }
    }

    public sealed class PatchExpenseCommandValidator : AbstractValidator<PatchExpenseCommand>
    {
        public PatchExpenseCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(Id)))
                .MaximumLength(ExpenseConstraints.IdMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(Id),
                        ExpenseConstraints.IdMaxLength
                    )
                );

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(Name)))
                .MaximumLength(ExpenseConstraints.NameMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(Name),
                        ExpenseConstraints.NameMaxLength
                    )
                );

            RuleFor(c => c.Description)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(Description)))
                .MaximumLength(ExpenseConstraints.DescriptionMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(Description),
                        ExpenseConstraints.DescriptionMaxLength
                    )
                );

            RuleFor(c => c.Value)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(Value)));

            RuleFor(c => c.CategoryId)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(CategoryId)))
                .MaximumLength(ExpenseConstraints.IdMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(CategoryId),
                        CategoryConstraints.IdMaxLength
                    )
                );
        }
    }
}
