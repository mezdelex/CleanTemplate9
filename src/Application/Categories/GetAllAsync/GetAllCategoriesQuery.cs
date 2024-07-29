using Application.Categories.Shared;
using MediatR;
using static Domain.Extensions.Collections.Collections;

namespace Application.Categories.GetAllAsync;

public record GetAllCategoriesQuery(int Page, int PageSize) : IRequest<PagedList<CategoryDTO>>;