namespace ProjectRouter.Application.Exceptions;

/// <summary>Thrown when an entity cannot be deleted because a competition still references it.</summary>
public class InUseException(string message) : Exception(message);
