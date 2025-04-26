namespace Domain.Exceptions;

public sealed class NotFoundException(string id)
    : Exception($"The entity with id {id} could not be found.") { }
