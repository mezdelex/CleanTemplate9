using Application.Shared;
using MediatR;

namespace Application.Categories.GetAsync;

public record GetCategoryQueryAsync(Guid id) : IRequest<CategoryDTO>;
