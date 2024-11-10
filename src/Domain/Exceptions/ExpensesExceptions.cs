namespace Domain.Exceptions;

public sealed class ExpenseNotFoundException : Exception
{
    public ExpenseNotFoundException(Guid id)
        : base($"The expense with id {id} could not be found.") { }
}
