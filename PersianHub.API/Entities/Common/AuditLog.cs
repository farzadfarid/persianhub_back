namespace PersianHub.API.Entities.Common;

/// <summary>
/// Persistent audit trail entry for business-critical actions.
/// Records who did what, to which entity, and when — for admin traceability,
/// payment accountability, and operational debugging.
///
/// Privacy notes:
/// - Passwords, tokens, and secrets are NEVER stored here.
/// - DetailsJson must contain only safe, non-sensitive contextual information.
/// - No FK to AppUser — logs remain even if a user is deleted, and logs can also
///   record system-initiated actions where PerformedByUserId is null.
/// </summary>
public class AuditLog
{
    public int Id { get; set; }

    /// <summary>Request-level correlation id (from X-Correlation-Id header or generated).</summary>
    public string? CorrelationId { get; set; }

    /// <summary>Well-known action name — use constants from <see cref="PersianHub.API.Common.AuditActions"/>.</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Name of the affected entity type (e.g. "Business", "Subscription").</summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>String-form id of the affected entity (nullable for auth events with no entity yet).</summary>
    public string? EntityId { get; set; }

    /// <summary>Id of the user who performed the action. Null for system or unauthenticated actions.</summary>
    public int? PerformedByUserId { get; set; }

    /// <summary>Role of the performing user at the time of the action.</summary>
    public string? PerformedByRole { get; set; }

    /// <summary>Optional JSON payload with safe contextual details (no secrets).</summary>
    public string? DetailsJson { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
