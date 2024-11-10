namespace Application.Features.Commands;

public sealed record PostExpenseCommand(
    string Name,
    string Description,
    double Value,
    Guid CategoryId
) : IRequest
{
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

    public sealed class PostExpenseCommandValidator : AbstractValidator<PostExpenseCommand>
    {
        private readonly int NameMaxLength = 30;
        private readonly int DescriptionMaxLength = 256;

        public PostExpenseCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeEmpty(nameof(PostExpenseCommand.Name))
                )
                .MaximumLength(NameMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(PostExpenseCommand.Name),
                        NameMaxLength
                    )
                );

            RuleFor(c => c.Description)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeEmpty(
                        nameof(PostExpenseCommand.Description)
                    )
                )
                .MaximumLength(DescriptionMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(PostExpenseCommand.Description),
                        DescriptionMaxLength
                    )
                );

            RuleFor(c => c.Value)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeEmpty(nameof(PostExpenseCommand.Value))
                );

            RuleFor(c => c.CategoryId)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeEmpty(
                        nameof(PostExpenseCommand.CategoryId)
                    )
                )
                .Must(x => x.GetType() == typeof(Guid))
                .WithMessage(
                    GenericValidationMessages.ShouldBeAGuid(nameof(PostExpenseCommand.CategoryId))
                );
        }
    }
}
