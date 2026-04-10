using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

public sealed class AdminAuditLogService(ApplicationDbContext db) : IAdminAuditLogService
{
    public async Task<PagedResult<AdminAuditLogListItemDto>> GetAllAsync(
        int? performedByUserId,
        string? action,
        string? entityType,
        string? entityId,
        string? correlationId,
        DateTime? fromUtc,
        DateTime? toUtc,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var query = db.AuditLogs.AsNoTracking().AsQueryable();

        if (performedByUserId.HasValue)
            query = query.Where(a => a.PerformedByUserId == performedByUserId.Value);

        if (!string.IsNullOrWhiteSpace(action))
            query = query.Where(a => a.Action == action);

        if (!string.IsNullOrWhiteSpace(entityType))
            query = query.Where(a => a.EntityType == entityType);

        if (!string.IsNullOrWhiteSpace(entityId))
            query = query.Where(a => a.EntityId == entityId);

        if (!string.IsNullOrWhiteSpace(correlationId))
            query = query.Where(a => a.CorrelationId == correlationId);

        if (fromUtc.HasValue)
            query = query.Where(a => a.CreatedAtUtc >= fromUtc.Value);

        if (toUtc.HasValue)
            query = query.Where(a => a.CreatedAtUtc <= toUtc.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(a => a.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new AdminAuditLogListItemDto(
                a.Id,
                a.CorrelationId,
                a.Action,
                a.EntityType,
                a.EntityId,
                a.PerformedByUserId,
                a.PerformedByRole,
                a.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminAuditLogListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminAuditLogDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var entry = await db.AuditLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, ct);

        if (entry is null)
            return Result<AdminAuditLogDetailDto>.Failure("Audit log entry not found.", ErrorCodes.NotFound);

        return Result<AdminAuditLogDetailDto>.Success(new AdminAuditLogDetailDto(
            entry.Id,
            entry.CorrelationId,
            entry.Action,
            entry.EntityType,
            entry.EntityId,
            entry.PerformedByUserId,
            entry.PerformedByRole,
            entry.DetailsJson,
            entry.CreatedAtUtc));
    }
}
