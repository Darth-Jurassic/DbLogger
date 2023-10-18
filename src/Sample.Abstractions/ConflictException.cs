namespace Sample.Abstractions;

/// <summary>
/// Conflict when saving changes, read data and try again. Similar to OptimisticConcurrencyException.
/// </summary>
public class ConflictException : SystemException
{
}