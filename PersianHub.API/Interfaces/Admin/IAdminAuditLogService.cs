using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminAuditLogService
{
    Task<PagedResult<AdminAuditLogListItemDto>> GetAllAsync(
        int? performedByUserId,
        string? action,
        string? entityType,
        string? entityId,
        string? correlationId,
        DateTime? fromUtc,
        DateTime? toUtc,
        int page,
        int pageSize,
        CancellationToken ct);

    Task<Result<AdminAuditLogDetailDto>> GetByIdAsync(int id, CancellationToken ct);
}
