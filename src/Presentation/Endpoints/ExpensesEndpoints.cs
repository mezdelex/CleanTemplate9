namespace Presentation.Endpoints;

public static class ExpensesEndpoints
{
    private static readonly ILogger _logger = new LoggerFactory().CreateLogger("Expenses");

    public static void MapExpensesEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/expenses/");

        group.MapGet("", GetAllExpensesQueryAsync).RequireAuthorization();
        group.MapGet("{id:guid}", GetExpenseQueryAsync).RequireAuthorization();
        group.MapPatch("", PatchExpenseCommandAsync).RequireAuthorization();
        group.MapPost("", PostExpenseCommandAsync).RequireAuthorization();
        group.MapDelete("{id:guid}", DeleteExpenseCommandAsync).RequireAuthorization();
    }

    public static async Task<IResult> GetAllExpensesQueryAsync(
        [FromQuery] string? name,
        [FromQuery] string? containedWord,
        [FromQuery] DateTime? minDate,
        [FromQuery] DateTime? maxDate,
        [FromQuery] Guid? categoryId,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        ISender sender
    )
    {
        try
        {
            return Results.Ok(
                await sender.Send(
                    new GetAllExpensesQuery
                    {
                        Name = name,
                        ContainedWord = containedWord,
                        MinDate = minDate,
                        MaxDate = maxDate,
                        CategoryId = categoryId,
                        Page = page,
                        PageSize = pageSize,
                    }
                )
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);

            return Results.BadRequest(e.Message);
        }
    }

    public static async Task<IResult> GetExpenseQueryAsync([FromRoute] Guid id, ISender sender)
    {
        try
        {
            return Results.Ok(await sender.Send(new GetExpenseQuery(id)));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);

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
            _logger.LogError(e.Message, e);

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
            _logger.LogError(e.Message, e);

            return Results.BadRequest(e.Message);
        }
    }

    public static async Task<IResult> DeleteExpenseCommandAsync([FromRoute] Guid id, ISender sender)
    {
        try
        {
            await sender.Send(new DeleteExpenseCommand(id));

            return Results.NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);

            return Results.BadRequest(e.Message);
        }
    }
}
