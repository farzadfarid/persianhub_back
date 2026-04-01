using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Entities.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Services.Layer1Hook;

public sealed class NotificationService(ApplicationDbContext db, IDateTimeProvider clock) : INotificationService
{
    public async Task<Result<NotificationDto>> CreateAsync(CreateNotificationDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Result<NotificationDto>.Failure("Title is required.", ErrorCodes.ValidationFailed);

        if (string.IsNullOrWhiteSpace(request.Message))
            return Result<NotificationDto>.Failure("Message is required.", ErrorCodes.ValidationFailed);

        var userExists = await db.AppUsers.AnyAsync(u => u.Id == request.AppUserId, ct);
        if (!userExists)
            return Result<NotificationDto>.Failure($"User with id {request.AppUserId} not found.", ErrorCodes.NotFound);

        var entity = new Notification
        {
            AppUserId = request.AppUserId,
            Type = request.Type,
            Title = request.Title.Trim(),
            Message = request.Message.Trim(),
            ReferenceType = request.ReferenceType,
            ReferenceId = request.ReferenceId,
            IsRead = false,
            CreatedAtUtc = clock.UtcNow
        };

        db.Notifications.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<NotificationDto>.Success(ToDto(entity));
    }

    public async Task<Result<NotificationDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Notifications.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id, ct);
        if (entity is null)
            return Result<NotificationDto>.Failure($"Notification with id {id} not found.", ErrorCodes.NotFound);

        return Result<NotificationDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<NotificationListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == appUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<NotificationListItemDto>>.Failure($"User with id {appUserId} not found.", ErrorCodes.NotFound);

        var items = await db.Notifications
            .AsNoTracking()
            .Where(n => n.AppUserId == appUserId)
            .OrderByDescending(n => n.CreatedAtUtc)
            .Select(n => new NotificationListItemDto(n.Id, n.Type, n.Title, n.IsRead, n.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<NotificationListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<NotificationListItemDto>>> GetUnreadByUserIdAsync(int appUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == appUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<NotificationListItemDto>>.Failure($"User with id {appUserId} not found.", ErrorCodes.NotFound);

        var items = await db.Notifications
            .AsNoTracking()
            .Where(n => n.AppUserId == appUserId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAtUtc)
            .Select(n => new NotificationListItemDto(n.Id, n.Type, n.Title, n.IsRead, n.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<NotificationListItemDto>>.Success(items);
    }

    public async Task<Result> MarkAsReadAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Notifications.FirstOrDefaultAsync(n => n.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Notification with id {id} not found.", ErrorCodes.NotFound);

        if (entity.IsRead)
            return Result.Success();

        entity.IsRead = true;
        entity.ReadAtUtc = clock.UtcNow;
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> MarkAllAsReadAsync(int appUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == appUserId, ct);
        if (!userExists)
            return Result.Failure($"User with id {appUserId} not found.", ErrorCodes.NotFound);

        var now = clock.UtcNow;
        await db.Notifications
            .Where(n => n.AppUserId == appUserId && !n.IsRead)
            .ExecuteUpdateAsync(s => s
                .SetProperty(n => n.IsRead, true)
                .SetProperty(n => n.ReadAtUtc, now), ct);

        return Result.Success();
    }

    private static NotificationDto ToDto(Notification n) => new(
        n.Id, n.AppUserId, n.Type, n.Title, n.Message,
        n.ReferenceType, n.ReferenceId, n.IsRead, n.ReadAtUtc, n.CreatedAtUtc);
}
