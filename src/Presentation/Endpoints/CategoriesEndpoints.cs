namespace Presentation.Endpoints;

public static class CategoriesEndpoints
{
    private static readonly ILogger _logger = new LoggerFactory().CreateLogger("Categories");

    public static void MapCategoriesEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/categories/");

        group.MapGet("", GetAllCategoriesQueryAsync).RequireAuthorization();
        group.MapGet("{id:guid}", GetCategoryQueryAsync).RequireAuthorization();
        group.MapPatch("", PatchCategoryCommandAsync).RequireAuthorization();
        group.MapPost("", PostCategoryCommandAsync).RequireAuthorization();
        group.MapDelete("{id:guid}", DeleteCategoryCommandAsync).RequireAuthorization();
    }

    public static async Task<IResult> GetAllCategoriesQueryAsync(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        ISender sender
    )
    {
        try
        {
            return Results.Ok(await sender.Send(new GetAllCategoriesQuery(page, pageSize)));
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
            return Results.Ok(await sender.Send(new GetCategoryQuery(id)));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);

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
            _logger.LogError(e.Message, e);

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
            _logger.LogError(e.Message, e);

            return Results.BadRequest(e.Message);
        }
    }

    public static async Task<IResult> DeleteCategoryCommandAsync(
        [FromRoute] Guid id,
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
            _logger.LogError(e.Message, e);

            return Results.BadRequest(e.Message);
        }
    }
}
