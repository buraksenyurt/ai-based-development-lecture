namespace CvApp.Domain.Exceptions;

/// <summary>
/// İstenen kaynak bulunamadığında fırlatılır. HTTP 404 Not Found ile eşleşir.
/// </summary>
public sealed class NotFoundException : Exception
{
    public NotFoundException(string resourceName, object key)
        : base($"{resourceName} bulunamadı. (Id: {key})") { }

    public NotFoundException(string message)
        : base(message) { }
}
