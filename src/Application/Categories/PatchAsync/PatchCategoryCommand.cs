using MediatR;

namespace Application.Categories.PatchAsync;

public record PatchCategoryCommand(Guid Id, string Name, string Description) : IRequest;
