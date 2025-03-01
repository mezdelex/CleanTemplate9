namespace Presentation.Endpoints;

public static class CategoriesEndpoints
{
    private static readonly ILogger _logger = new LoggerFactory().CreateLogger("Categories");

    public static void MapCategoriesEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/categories/");

        group.MapGet(string.Empty, GetAllCategoriesQueryAsync).RequireAuthorization();
        group.MapGet(Patterns.IdAsGuidPattern, GetCategoryQueryAsync).RequireAuthorization();
        group.MapPatch(string.Empty, PatchCategoryCommandAsync).RequireAuthorization();
        group.MapPost(string.Empty, PostCategoryCommandAsync).RequireAuthorization();
        group
            .MapDelete(Patterns.IdAsGuidPattern, DeleteCategoryCommandAsync)
            .RequireAuthorization();
    }

    public static async Task<IResult> GetAllCategoriesQueryAsync(
        [FromQuery] GetAllCategoriesQuery query,
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

    public static async Task<IResult> GetCategoryQueryAsync(
        [FromRoute] GetCategoryQuery query,
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
        [FromRoute] DeleteCategoryCommand command,
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
}
