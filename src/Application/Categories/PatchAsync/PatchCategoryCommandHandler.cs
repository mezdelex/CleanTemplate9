using Application.Abstractions;
using Domain.Categories;
using Domain.Persistence;
using FluentValidation;
using MediatR;

namespace Application.Categories.PatchAsync;

public sealed class PatchCategoryCommandHandler : IRequestHandler<PatchCategoryCommand>
{
    private readonly IValidator<PatchCategoryCommand> _validator;
    private readonly ICategoriesRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IEventBus _eventBus;

    public PatchCategoryCommandHandler(
        IValidator<PatchCategoryCommand> validator,
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

    public async Task Handle(PatchCategoryCommand request, CancellationToken cancellationToken)
    {
        var results = await _validator.ValidateAsync(request, cancellationToken);
        if (!results.IsValid)
            throw new ValidationException(results.ToString().Replace("\r\n", " "));

        var categoryToPatch = new Category(request.Id, request.Name, request.Description);

        await _repository.PatchAsync(categoryToPatch, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        await _eventBus.PublishAsync(
            new PatchedCategoryEvent(
                categoryToPatch.Id,
                categoryToPatch.Name,
                categoryToPatch.Description
            ),
            cancellationToken
        );
    }
}
