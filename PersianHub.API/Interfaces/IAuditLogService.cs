namespace PersianHub.API.Interfaces;

/// <summary>
/// Centralized audit logging service.
/// Writes persistent audit entries for business-critical actions.
///
/// Contract:
/// - WriteAsync must never throw — failures are caught internally and logged via ILogger.
///   Audit log errors must not break the calling business flow.
/// - Correlation ID and current user are resolved internally from HttpContext.
/// - DetailsJson must never contain passwords, tokens, secrets, or hashed credentials.
/// </summary>
public interface IAuditLogService
{
    /// <summary>
    /// Writes a persistent audit entry.
    /// </summary>
    /// <param name="action">Well-known action name — use <see cref="PersianHub.API.Common.AuditActions"/> constants.</param>
    /// <param name="entityType">Name of the affected entity type (e.g. "Business", "Subscription").</param>
    /// <param name="entityId">String-form id of the affected entity. Null for auth events with no entity yet.</param>
    /// <param name="details">Optional safe context object — serialized to JSON. Must not contain secrets.</param>
    /// <param name="ct">Cancellation token.</param>
    Task WriteAsync(string action, string entityType, string? entityId, object? details = null, CancellationToken ct = default);
}
