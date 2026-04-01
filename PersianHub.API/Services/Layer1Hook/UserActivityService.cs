using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Entities.Layer1Hook;
using PersianHub.API.Enums.Common;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Services.Layer1Hook;

public sealed class UserActivityService(ApplicationDbContext db, IDateTimeProvider clock) : IUserActivityService
{
    public async Task<Result<UserActivityDto>> CreateAsync(CreateUserActivityDto request, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == request.AppUserId, ct);
        if (!userExists)
            return Result<UserActivityDto>.Failure($"User with id {request.AppUserId} not found.", ErrorCodes.NotFound);

        var entity = new UserActivity
        {
            AppUserId = request.AppUserId,
            ActivityType = request.ActivityType,
            ReferenceType = request.ReferenceType,
            ReferenceId = request.ReferenceId,
            Metadata = request.Metadata?.Trim(),
            CreatedAtUtc = clock.UtcNow
        };

        db.UserActivities.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<UserActivityDto>.Success(ToDto(entity));
    }

    public async Task<Result<UserActivityDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.UserActivities.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
        if (entity is null)
            return Result<UserActivityDto>.Failure($"UserActivity with id {id} not found.", ErrorCodes.NotFound);

        return Result<UserActivityDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<UserActivityListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == appUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<UserActivityListItemDto>>.Failure($"User with id {appUserId} not found.", ErrorCodes.NotFound);

        var items = await db.UserActivities
            .AsNoTracking()
            .Where(u => u.AppUserId == appUserId)
            .OrderByDescending(u => u.CreatedAtUtc)
            .Select(u => new UserActivityListItemDto(u.Id, u.ActivityType, u.ReferenceType, u.ReferenceId, u.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<UserActivityListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<UserActivityListItemDto>>> GetByReferenceAsync(ReferenceType referenceType, int referenceId, CancellationToken ct = default)
    {
        var items = await db.UserActivities
            .AsNoTracking()
            .Where(u => u.ReferenceType == referenceType && u.ReferenceId == referenceId)
            .OrderByDescending(u => u.CreatedAtUtc)
            .Select(u => new UserActivityListItemDto(u.Id, u.ActivityType, u.ReferenceType, u.ReferenceId, u.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<UserActivityListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<UserActivityListItemDto>>> GetRecentByUserIdAsync(int appUserId, int count, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == appUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<UserActivityListItemDto>>.Failure($"User with id {appUserId} not found.", ErrorCodes.NotFound);

        var items = await db.UserActivities
            .AsNoTracking()
            .Where(u => u.AppUserId == appUserId)
            .OrderByDescending(u => u.CreatedAtUtc)
            .Take(count > 0 ? count : 20)
            .Select(u => new UserActivityListItemDto(u.Id, u.ActivityType, u.ReferenceType, u.ReferenceId, u.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<UserActivityListItemDto>>.Success(items);
    }

    private static UserActivityDto ToDto(UserActivity u) => new(
        u.Id, u.AppUserId, u.ActivityType, u.ReferenceType, u.ReferenceId, u.Metadata, u.CreatedAtUtc);
}
