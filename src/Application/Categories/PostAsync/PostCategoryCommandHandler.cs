using Domain.Categories;
using Domain.Persistence;
using FluentValidation;
using MediatR;

namespace Application.Categories.PostAsync;

public sealed class PostCategoryCommandHandler : IRequestHandler<PostCategoryCommand>
{
    private readonly IValidator<PostCategoryCommand> _validator;
    private readonly ICategoriesRepository _repository;
    private readonly IUnitOfWork _uow;

    public PostCategoryCommandHandler(
        IValidator<PostCategoryCommand> validator,
        ICategoriesRepository repository,
        IUnitOfWork uow
    )
    {
        _validator = validator;
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(PostCategoryCommand request, CancellationToken cancellationToken)
    {
        var results = await _validator.ValidateAsync(request, cancellationToken);
        if (!results.IsValid)
            throw new ValidationException(results.ToString().Replace("\r\n", " "));

        await _repository.PostAsync(
            new Category(Guid.NewGuid(), request.Name, request.Description),
            cancellationToken
        );

        await _uow.SaveChangesAsync(cancellationToken);
    }
}