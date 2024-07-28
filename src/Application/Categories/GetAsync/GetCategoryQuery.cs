using Application.Categories.Shared;
using MediatR;

namespace Application.Categories.GetAsync;

public record GetCategoryQuery(Guid id) : IRequest<CategoryDTO>;
