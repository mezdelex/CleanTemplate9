namespace Presentation.Endpoints;

public static class ApplicationUsersEndpoints
{
    private static readonly ILogger _logger = new LoggerFactory().CreateLogger("ApplicationUsers");

    public static void MapApplicationUsersEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(MapGroups.ApplicationUsers);

        group.MapPost(Patterns.AllPattern, GetAllApplicationUsersQueryAsync).RequireAuthorization();
        group.MapGet(Patterns.IdPattern, GetApplicationUserQueryAsync).RequireAuthorization();
    }

    public static async Task<IResult> GetAllApplicationUsersQueryAsync(
        [FromBody] GetAllApplicationUsersQuery query,
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

    public static async Task<IResult> GetApplicationUserQueryAsync(
        [FromRoute] string id,
        ISender sender
    )
    {
        try
        {
            return Results.Ok(await sender.Send(new GetApplicationUserQuery(id)));
        }
        catch (Exception e)
        {
            _logger.LogError(Errors.ErrorMessageTemplate, e, e.Message);

            return Results.NotFound(e.Message);
        }
    }
}
