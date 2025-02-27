namespace Application.Features.Commands;

public sealed record PatchCategoryCommand(Guid Id, string Name, string Description) : IRequest
{
    public sealed class PatchCategoryCommandHandler : IRequestHandler<PatchCategoryCommand>
    {
        private readonly IValidator<PatchCategoryCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ICategoriesRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public PatchCategoryCommandHandler(
            IValidator<PatchCategoryCommand> validator,
            IMapper mapper,
            ICategoriesRepository repository,
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

        public async Task Handle(PatchCategoryCommand request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
                throw new ValidationException(results.ToString().Replace("\r\n", " "));

            var categoryToPatch = _mapper.Map<Category>(request);

            await _repository.PatchAsync(categoryToPatch, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            await _eventBus.PublishAsync(
                _mapper.Map<PatchedCategoryEvent>(categoryToPatch),
                cancellationToken
            );
        }
    }

    public sealed class PatchCategoryCommandValidator : AbstractValidator<PatchCategoryCommand>
    {
        public PatchCategoryCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(Id)))
                .Must(id => id.GetType().Equals(typeof(Guid)))
                .WithMessage(GenericValidationMessages.ShouldBeAGuid(nameof(Id)));

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(Name)))
                .MaximumLength(CategoryConstraints.NameMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(Name),
                        CategoryConstraints.NameMaxLength
                    )
                );

            RuleFor(c => c.Description)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty(nameof(Description)))
                .MaximumLength(CategoryConstraints.DescriptionMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(Description),
                        CategoryConstraints.DescriptionMaxLength
                    )
                );
        }
    }
}
