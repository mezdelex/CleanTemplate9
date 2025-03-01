namespace Presentation.Endpoints;

public static class ExpensesEndpoints
{
    private static readonly ILogger _logger = new LoggerFactory().CreateLogger("Expenses");

    public static void MapExpensesEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/expenses/");

        group.MapGet(string.Empty, GetAllExpensesQueryAsync).RequireAuthorization();
        group.MapGet(Patterns.IdAsGuidPattern, GetExpenseQueryAsync).RequireAuthorization();
        group.MapPatch(string.Empty, PatchExpenseCommandAsync).RequireAuthorization();
        group.MapPost(string.Empty, PostExpenseCommandAsync).RequireAuthorization();
        group.MapDelete(Patterns.IdAsGuidPattern, DeleteExpenseCommandAsync).RequireAuthorization();
    }

    public static async Task<IResult> GetAllExpensesQueryAsync(
        [FromQuery] GetAllExpensesQuery query,
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

    public static async Task<IResult> GetExpenseQueryAsync(
        [FromRoute] GetExpenseQuery query,
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

    public static async Task<IResult> PatchExpenseCommandAsync(
        [FromBody] PatchExpenseCommand command,
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

    public static async Task<IResult> PostExpenseCommandAsync(
        [FromBody] PostExpenseCommand command,
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

    public static async Task<IResult> DeleteExpenseCommandAsync(
        [FromRoute] DeleteExpenseCommand command,
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
