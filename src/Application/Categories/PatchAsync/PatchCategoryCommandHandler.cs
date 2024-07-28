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

    public PatchCategoryCommandHandler(
        IValidator<PatchCategoryCommand> validator,
        ICategoriesRepository repository,
        IUnitOfWork uow
    )
    {
        _validator = validator;
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(PatchCategoryCommand request, CancellationToken cancellationToken)
    {
        var results = await _validator.ValidateAsync(request, cancellationToken);
        if (!results.IsValid)
            throw new ValidationException(results.ToString().Replace("\r\n", " "));

        await _repository.PatchAsync(
            new Category(request.Id, request.Name, request.Description),
            cancellationToken
        );

        await _uow.SaveChangesAsync(cancellationToken);
    }
}
