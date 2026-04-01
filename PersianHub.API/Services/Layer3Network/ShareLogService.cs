using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Entities.Layer3Network;
using PersianHub.API.Enums.Common;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Services.Layer3Network;

public sealed class ShareLogService(ApplicationDbContext db, IDateTimeProvider clock) : IShareLogService
{
    public async Task<Result<ShareLogDto>> CreateAsync(CreateShareLogDto request, CancellationToken ct = default)
    {
        if (request.AppUserId.HasValue)
        {
            var userExists = await db.AppUsers.AnyAsync(u => u.Id == request.AppUserId.Value, ct);
            if (!userExists)
                return Result<ShareLogDto>.Failure($"User with id {request.AppUserId.Value} not found.", ErrorCodes.NotFound);
        }

        var entity = new ShareLog
        {
            AppUserId = request.AppUserId,
            ShareType = request.ShareType,
            Channel = request.Channel,
            ReferenceType = request.ReferenceType,
            ReferenceId = request.ReferenceId,
            CreatedAtUtc = clock.UtcNow
        };

        db.ShareLogs.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<ShareLogDto>.Success(ToDto(entity));
    }

    public async Task<Result<ShareLogDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.ShareLogs.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);
        if (entity is null)
            return Result<ShareLogDto>.Failure($"ShareLog with id {id} not found.", ErrorCodes.NotFound);

        return Result<ShareLogDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<ShareLogListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == appUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<ShareLogListItemDto>>.Failure($"User with id {appUserId} not found.", ErrorCodes.NotFound);

        var items = await db.ShareLogs
            .AsNoTracking()
            .Where(s => s.AppUserId == appUserId)
            .OrderByDescending(s => s.CreatedAtUtc)
            .Select(s => new ShareLogListItemDto(s.Id, s.AppUserId, s.ShareType, s.Channel, s.ReferenceType, s.ReferenceId, s.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<ShareLogListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<ShareLogListItemDto>>> GetByReferenceAsync(ReferenceType referenceType, int referenceId, CancellationToken ct = default)
    {
        var items = await db.ShareLogs
            .AsNoTracking()
            .Where(s => s.ReferenceType == referenceType && s.ReferenceId == referenceId)
            .OrderByDescending(s => s.CreatedAtUtc)
            .Select(s => new ShareLogListItemDto(s.Id, s.AppUserId, s.ShareType, s.Channel, s.ReferenceType, s.ReferenceId, s.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<ShareLogListItemDto>>.Success(items);
    }

    private static ShareLogDto ToDto(ShareLog s) => new(
        s.Id, s.AppUserId, s.ShareType, s.Channel, s.ReferenceType, s.ReferenceId, s.CreatedAtUtc);
}
