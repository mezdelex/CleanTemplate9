using MediatR;

namespace Application.Categories.PostAsync;

public record PostCategoryCommand(string Name, string Description) : IRequest;