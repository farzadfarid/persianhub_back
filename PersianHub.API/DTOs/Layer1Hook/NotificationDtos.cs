using PersianHub.API.Enums.Layer1Hook;
using PersianHub.API.Enums.Common;

namespace PersianHub.API.DTOs.Layer1Hook;

public record CreateNotificationDto(
    int AppUserId,
    NotificationType Type,
    string Title,
    string Message,
    ReferenceType? ReferenceType = null,
    int? ReferenceId = null
);

public record NotificationDto(
    int Id,
    int AppUserId,
    NotificationType Type,
    string Title,
    string Message,
    ReferenceType? ReferenceType,
    int? ReferenceId,
    bool IsRead,
    DateTime? ReadAtUtc,
    DateTime CreatedAtUtc
);

public record NotificationListItemDto(
    int Id,
    NotificationType Type,
    string Title,
    bool IsRead,
    DateTime CreatedAtUtc
);
