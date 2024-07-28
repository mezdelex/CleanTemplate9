using Application.Categories.GetAllAsync;
using Application.Categories.GetAsync;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Presentation.Endpoints;

public static class CategoriesEndpoints
{
    private static readonly ILogger _logger = new LoggerFactory().CreateLogger("Categories");

    public static void MapCategoriesEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/categories/");

        group.MapGet("", GetAllCategoriesQueryAsync).RequireAuthorization();
        group.MapGet("{id:guid}", GetCategoryQueryAsync).RequireAuthorization();
        /* group.MapPatch("", CreateUserAsync); */
        /* group.MapPost("", CreateUserAsync); */
        /* group.MapDelete("", CreateUserAsync); */
    }

    public static async Task<IResult> GetAllCategoriesQueryAsync(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        ISender sender
    )
    {
        try
        {
            return Results.Ok(await sender.Send(new GetAllCategoriesQueryAsync(page, pageSize)));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);

            return Results.BadRequest(e.Message);
        }
    }

    public static async Task<IResult> GetCategoryQueryAsync([FromRoute] Guid id, ISender sender)
    {
        try
        {
            return Results.Ok(await sender.Send(new GetCategoryQueryAsync(id)));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);

            return Results.NotFound(e.Message);
        }
    }
}
