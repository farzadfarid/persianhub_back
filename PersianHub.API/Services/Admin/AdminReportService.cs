using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

public sealed class AdminReportService(ApplicationDbContext db) : IAdminReportService
{
    public async Task<PagedResult<AdminReportListItemDto>> GetAllAsync(
        ReportStatus? status, string? referenceType, int? referenceId, int? userId,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.Reports
            .AsNoTracking()
            .Include(r => r.AppUser)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        if (!string.IsNullOrWhiteSpace(referenceType) &&
            Enum.TryParse<PersianHub.API.Enums.Common.ReferenceType>(referenceType, true, out var refType))
            query = query.Where(r => r.ReferenceType == refType);

        if (referenceId.HasValue)
            query = query.Where(r => r.ReferenceId == referenceId.Value);

        if (userId.HasValue)
            query = query.Where(r => r.AppUserId == userId.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(r => r.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new AdminReportListItemDto(
                r.Id, r.AppUserId,
                r.AppUser != null ? r.AppUser.Email : null,
                r.ReferenceType, r.ReferenceId,
                r.Reason, r.Status, r.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminReportListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminReportDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var report = await db.Reports
            .AsNoTracking()
            .Include(r => r.AppUser)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (report is null)
            return Result<AdminReportDetailDto>.Failure("Report not found.", ErrorCodes.NotFound);

        return Result<AdminReportDetailDto>.Success(new AdminReportDetailDto(
            report.Id, report.AppUserId,
            report.AppUser?.Email,
            report.ReferenceType, report.ReferenceId,
            report.Reason, report.Details,
            report.Status, report.CreatedAtUtc, report.UpdatedAtUtc));
    }

    public async Task<Result> ResolveAsync(int id, CancellationToken ct)
    {
        var report = await db.Reports.FindAsync([id], ct);
        if (report is null)
            return Result.Failure("Report not found.", ErrorCodes.NotFound);

        report.Status = ReportStatus.ActionTaken;
        report.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> MarkReviewedAsync(int id, CancellationToken ct)
    {
        var report = await db.Reports.FindAsync([id], ct);
        if (report is null)
            return Result.Failure("Report not found.", ErrorCodes.NotFound);

        if (report.Status != ReportStatus.Pending)
            return Result.Failure("Only pending reports can be marked as reviewed.", ErrorCodes.ValidationFailed);

        report.Status = ReportStatus.Reviewed;
        report.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> DismissAsync(int id, CancellationToken ct)
    {
        var report = await db.Reports.FindAsync([id], ct);
        if (report is null)
            return Result.Failure("Report not found.", ErrorCodes.NotFound);

        report.Status = ReportStatus.Dismissed;
        report.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
