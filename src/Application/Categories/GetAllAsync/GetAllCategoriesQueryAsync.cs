using Application.Shared;
using MediatR;
using static Domain.Extensions.Collections.Collections;

namespace Application.Categories.GetAllAsync;

public record GetAllCategoriesQueryAsync(int Page, int PageSize) : IRequest<PagedList<CategoryDTO>>;
