using MediatR;

namespace Application.Categories.DeleteAsync;

public record DeleteCategoryCommand(Guid id) : IRequest;
