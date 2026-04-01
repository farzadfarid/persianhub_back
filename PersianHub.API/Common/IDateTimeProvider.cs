namespace PersianHub.API.Common;

/// <summary>
/// Abstracts the system clock to make date-dependent logic testable.
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
