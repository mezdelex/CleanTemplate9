namespace Application.Features.Commands;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest
{
    public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoriesRepository _repository;
        private readonly IUnitOfWork _uow;

        public DeleteCategoryCommandHandler(ICategoriesRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(GenericValidationMessages.ShouldNotBeEmpty("Id"));
        }
    }
}
