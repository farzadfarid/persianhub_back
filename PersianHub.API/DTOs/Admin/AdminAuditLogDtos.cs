namespace PersianHub.API.DTOs.Admin;

public record AdminAuditLogListItemDto(
    int Id,
    string? CorrelationId,
    string Action,
    string EntityType,
    string? EntityId,
    int? PerformedByUserId,
    string? PerformedByRole,
    DateTime CreatedAtUtc);

public record AdminAuditLogDetailDto(
    int Id,
    string? CorrelationId,
    string Action,
    string EntityType,
    string? EntityId,
    int? PerformedByUserId,
    string? PerformedByRole,
    string? DetailsJson,
    DateTime CreatedAtUtc);
