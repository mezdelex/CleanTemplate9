namespace Presentation.Endpoints;

public static class CategoriesEndpoints
{
    private static readonly ILogger _logger = new LoggerFactory().CreateLogger("Categories");

    public static void MapCategoriesEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(MapGroups.Categories);

        group.MapPost(Patterns.AllPattern, GetAllCategoriesQueryAsync).RequireAuthorization();
        group.MapGet(Patterns.IdPattern, GetCategoryQueryAsync).RequireAuthorization();
        group.MapPatch(string.Empty, PatchCategoryCommandAsync).RequireAuthorization();
        group.MapPost(string.Empty, PostCategoryCommandAsync).RequireAuthorization();
        group.MapDelete(Patterns.IdPattern, DeleteCategoryCommandAsync).RequireAuthorization();
    }

    public static async Task<IResult> GetAllCategoriesQueryAsync(
        [FromBody] GetAllCategoriesQuery query,
        ISender sender
    )
    {
        try
        {
            return Results.Ok(await sender.Send(query));
        }
        catch (Exception e)
        {
            _logger.LogError(Errors.ErrorMessageTemplate, e, e.Message);

            return Results.BadRequest(e.Message);
        }
    }

    public static async Task<IResult> GetCategoryQueryAsync([FromRoute] string id, ISender sender)
    {
        try
        {
            return Results.Ok(await sender.Send(new GetCategoryQuery(id)));
        }
        catch (Exception e)
        {
            _logger.LogError(Errors.ErrorMessageTemplate, e, e.Message);

            return Results.NotFound(e.Message);
        }
    }

    public static async Task<IResult> PatchCategoryCommandAsync(
        [FromBody] PatchCategoryCommand command,
        ISender sender
    )
    {
        try
        {
            await sender.Send(command);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(Errors.ErrorMessageTemplate, e, e.Message);

            return Results.BadRequest(e.Message);
        }
    }

    public static async Task<IResult> PostCategoryCommandAsync(
        [FromBody] PostCategoryCommand command,
        ISender sender
    )
    {
        try
        {
            await sender.Send(command);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(Errors.ErrorMessageTemplate, e, e.Message);

            return Results.BadRequest(e.Message);
        }
    }

    public static async Task<IResult> DeleteCategoryCommandAsync(
        [FromRoute] string id,
        ISender sender
    )
    {
        try
        {
            await sender.Send(new DeleteCategoryCommand(id));

            return Results.NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(Errors.ErrorMessageTemplate, e, e.Message);

            return Results.BadRequest(e.Message);
        }
    }
}
