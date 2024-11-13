namespace Application.Features.Commands;

public sealed record PatchExpenseCommand(
    Guid Id,
    string Name,
    string Description,
    double Value,
    Guid CategoryId
) : IRequest
{
    public sealed class PatchExpenseCommandHandler : IRequestHandler<PatchExpenseCommand>
    {
        private readonly IValidator<PatchExpenseCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IExpensesRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public PatchExpenseCommandHandler(
            IValidator<PatchExpenseCommand> validator,
            IMapper mapper,
            IExpensesRepository repository,
            IUnitOfWork uow,
            IEventBus eventBus
        )
        {
            _validator = validator;
            _mapper = mapper;
            _repository = repository;
            _uow = uow;
            _eventBus = eventBus;
        }

        public async Task Handle(PatchExpenseCommand request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
                throw new ValidationException(results.ToString().Replace("\r\n", " "));

            var expenseToPatch = _mapper.Map<Expense>(request);

            await _repository.PatchAsync(expenseToPatch, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            await _eventBus.PublishAsync(
                _mapper.Map<PatchedExpenseEvent>(expenseToPatch),
                cancellationToken
            );
        }
    }

    public sealed class PatchExpenseCommandValidator : AbstractValidator<PatchExpenseCommand>
    {
        private readonly int NameMaxLength = 30;
        private readonly int DescriptionMaxLength = 256;

        public PatchExpenseCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeEmpty(nameof(PatchExpenseCommand.Id))
                )
                .Must(id => id.GetType() == typeof(Guid))
                .WithMessage(
                    GenericValidationMessages.ShouldBeAGuid(nameof(PatchExpenseCommand.Id))
                );

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeEmpty(nameof(PatchExpenseCommand.Name))
                )
                .MaximumLength(NameMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(PatchExpenseCommand.Name),
                        NameMaxLength
                    )
                );

            RuleFor(c => c.Description)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeEmpty(
                        nameof(PatchExpenseCommand.Description)
                    )
                )
                .MaximumLength(DescriptionMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(PatchExpenseCommand.Description),
                        DescriptionMaxLength
                    )
                );

            RuleFor(c => c.Value)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeEmpty(nameof(PatchExpenseCommand.Value))
                );

            RuleFor(c => c.CategoryId)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeEmpty(
                        nameof(PatchExpenseCommand.CategoryId)
                    )
                );
        }
    }
}
