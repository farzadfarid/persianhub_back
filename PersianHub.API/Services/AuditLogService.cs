using System.Text.Json;
using PersianHub.API.Auth;
using PersianHub.API.Data;
using PersianHub.API.Entities.Common;
using PersianHub.API.Interfaces;

namespace PersianHub.API.Services;

/// <summary>
/// Persists audit entries to the AuditLogs table.
///
/// Current user identity and correlation ID are resolved from HttpContext so callers
/// only need to supply the action, entity type/id, and optional context details.
///
/// Failures are caught and logged via ILogger — audit errors never surface to the caller.
/// </summary>
public sealed class AuditLogService(
    ApplicationDbContext db,
    ICurrentUserService currentUser,
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuditLogService> logger) : IAuditLogService
{
    private static readonly JsonSerializerOptions _jsonOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task WriteAsync(
        string action,
        string entityType,
        string? entityId,
        object? details = null,
        CancellationToken ct = default)
    {
        try
        {
            var correlationId = httpContextAccessor.HttpContext?.Items["CorrelationId"] as string;

            var userId = currentUser.GetUserId();
            var role = currentUser.GetRole();

            string? detailsJson = null;
            if (details is not null)
            {
                try
                {
                    detailsJson = JsonSerializer.Serialize(details, _jsonOptions);
                }
                catch
                {
                    // If serialization fails, omit details rather than failing the audit write.
                    detailsJson = null;
                }
            }

            var entry = new AuditLog
            {
                CorrelationId = correlationId,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                PerformedByUserId = userId == 0 ? null : userId,
                PerformedByRole = string.IsNullOrEmpty(role) ? null : role,
                DetailsJson = detailsJson,
                CreatedAtUtc = DateTime.UtcNow
            };

            db.AuditLogs.Add(entry);
            await db.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            // Audit failures must not propagate — log and continue.
            logger.LogError(ex, "Failed to write audit log. Action={Action} EntityType={EntityType} EntityId={EntityId}",
                action, entityType, entityId);
        }
    }
}
