namespace LoopLab.Domain.Common;

/// <summary>
/// Domain kurallarından biri ihlal edildiğinde fırlatılır.
/// </summary>
public sealed class DomainException(string message) : Exception(message);
