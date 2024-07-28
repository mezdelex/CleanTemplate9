using Application.Abstractions;
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

        var categoryToPost = new Category(Guid.NewGuid(), request.Name, request.Description);

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
