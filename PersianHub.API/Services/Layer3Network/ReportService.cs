using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Entities.Layer3Network;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Services.Layer3Network;

public sealed class ReportService(ApplicationDbContext db, IDateTimeProvider clock) : IReportService
{
    public async Task<Result<ReportCreatedDto>> CreateAsync(CreateReportDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Reason))
            return Result<ReportCreatedDto>.Failure("Reason is required.", ErrorCodes.ValidationFailed);

        if (request.AppUserId.HasValue)
        {
            var userExists = await db.AppUsers.AnyAsync(u => u.Id == request.AppUserId.Value, ct);
            if (!userExists)
                return Result<ReportCreatedDto>.Failure($"User with id {request.AppUserId.Value} not found.", ErrorCodes.NotFound);
        }

        var now = clock.UtcNow;
        var entity = new Report
        {
            AppUserId = request.AppUserId,
            ReferenceType = request.ReferenceType,
            ReferenceId = request.ReferenceId,
            Reason = request.Reason.Trim(),
            Details = request.Details?.Trim(),
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.Reports.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<ReportCreatedDto>.Success(new ReportCreatedDto(entity.Id, entity.Status, entity.CreatedAtUtc));
    }
}
