namespace ProjectRouter.Application.Exceptions;

public class NotFoundException(string entityName, Guid id)
    : Exception($"{entityName} '{id}' was not found.");
