using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer1Hook;

namespace PersianHub.API.Interfaces.Layer1Hook;

public interface INotificationService
{
    Task<Result<NotificationDto>> CreateAsync(CreateNotificationDto request, CancellationToken ct = default);
    Task<Result<NotificationDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<NotificationListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<NotificationListItemDto>>> GetUnreadByUserIdAsync(int appUserId, CancellationToken ct = default);
    Task<Result> MarkAsReadAsync(int id, CancellationToken ct = default);
    Task<Result> MarkAllAsReadAsync(int appUserId, CancellationToken ct = default);
}
