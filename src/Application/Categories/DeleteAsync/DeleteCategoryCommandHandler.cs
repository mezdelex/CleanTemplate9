using Domain.Categories;
using Domain.Persistence;
using MediatR;

namespace Application.Categories.DeleteAsync;

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
        await _repository.DeleteAsync(request.id, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
