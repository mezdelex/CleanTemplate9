namespace Domain.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(Guid id)
        : base($"The entity with id {id} could not be found.") { }
}
