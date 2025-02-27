namespace Application.Features.Commands;

public sealed record PostCategoryCommand(string Name, string Description) : IRequest
{
    public sealed class PostCategoryCommandHandler : IRequestHandler<PostCategoryCommand>
    {
        private readonly IValidator<PostCategoryCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ICategoriesRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public PostCategoryCommandHandler(
            IValidator<PostCategoryCommand> validator,
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

        public async Task Handle(PostCategoryCommand request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
                throw new ValidationException(results.ToString().Replace("\r\n", " "));

            var categoryToPost = _mapper.Map<Category>(request);

            await _repository.PostAsync(categoryToPost, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            await _eventBus.PublishAsync(
                _mapper.Map<PostedCategoryEvent>(categoryToPost),
                cancellationToken
            );
        }
    }

    public sealed class PostCategoryCommandValidator : AbstractValidator<PostCategoryCommand>
    {
        public PostCategoryCommandValidator()
        {
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
