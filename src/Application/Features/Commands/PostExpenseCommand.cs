namespace Application.Features.Commands;

public sealed record PostExpenseCommand(
    string Name,
    string Description,
    double Value,
    string CategoryId,
    string ApplicationUserId
) : IRequest
{
    public sealed class PostExpenseCommandHandler(
        IValidator<PostExpenseCommand> validator,
        IMapper mapper,
        IExpensesRepository repository,
        IUnitOfWork uow,
        IRedisCache redisCache,
        IEventBus eventBus
    ) : IRequestHandler<PostExpenseCommand>
    {
        private readonly IValidator<PostExpenseCommand> _validator = validator;
        private readonly IMapper _mapper = mapper;
        private readonly IExpensesRepository _repository = repository;
        private readonly IUnitOfWork _uow = uow;
        private readonly IRedisCache _redisCache = redisCache;
        private readonly IEventBus _eventBus = eventBus;

        public async Task Handle(PostExpenseCommand request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
                throw new ValidationException(results.ToString().Replace("\r\n", " "));

            var expenseToPost = _mapper.Map<Expense>(request);

            await _repository.PostAsync(expenseToPost, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            await _redisCache.RemoveKeysByPattern(nameof(Expense));
            await _eventBus.PublishAsync(
                _mapper.Map<PostedExpenseEvent>(expenseToPost),
                cancellationToken
            );
        }
    }

    public sealed class PostExpenseCommandValidator : AbstractValidator<PostExpenseCommand>
    {
        public PostExpenseCommandValidator()
        {
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
                .MaximumLength(CategoryConstraints.IdMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(CategoryId),
                        CategoryConstraints.IdMaxLength
                    )
                );

            RuleFor(c => c.ApplicationUserId)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(ApplicationUserId)))
                .MaximumLength(ApplicationUserConstraints.IdMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(ApplicationUserId),
                        ApplicationUserConstraints.IdMaxLength
                    )
                );
        }
    }
}
