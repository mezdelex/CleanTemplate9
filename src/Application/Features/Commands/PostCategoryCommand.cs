namespace Application.Features.Commands;

public sealed record PostCategoryCommand(string Name, string Description) : IRequest
{
    public sealed class PostCategoryCommandHandler : IRequestHandler<PostCategoryCommand>
    {
        private readonly IValidator<PostCategoryCommand> _validator;
        private readonly ICategoriesRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public PostCategoryCommandHandler(
            IValidator<PostCategoryCommand> validator,
            ICategoriesRepository repository,
            IUnitOfWork uow,
            IEventBus eventBus
        )
        {
            _validator = validator;
            _repository = repository;
            _uow = uow;
            _eventBus = eventBus;
        }

        public async Task Handle(PostCategoryCommand request, CancellationToken cancellationToken)
        {
            var results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
                throw new ValidationException(results.ToString().Replace("\r\n", " "));

            var categoryToPost = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
            };

            await _repository.PostAsync(categoryToPost, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            await _eventBus.PublishAsync(
                new PostedCategoryEvent(
                    categoryToPost.Id,
                    categoryToPost.Name,
                    categoryToPost.Description
                ),
                cancellationToken
            );
        }
    }

    public sealed class PostCategoryCommandValidator : AbstractValidator<PostCategoryCommand>
    {
        private readonly int NameMaxLength = 30;
        private readonly int DescriptionMaxLength = 256;

        public PostCategoryCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeEmpty(nameof(PostCategoryCommand.Name))
                )
                .MaximumLength(NameMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(PostCategoryCommand.Name),
                        NameMaxLength
                    )
                );

            RuleFor(c => c.Description)
                .NotEmpty()
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeEmpty(
                        nameof(PostCategoryCommand.Description)
                    )
                )
                .MaximumLength(DescriptionMaxLength)
                .WithMessage(
                    GenericValidationMessages.ShouldNotBeLongerThan(
                        nameof(PostCategoryCommand.Description),
                        DescriptionMaxLength
                    )
                );
        }
    }
}
