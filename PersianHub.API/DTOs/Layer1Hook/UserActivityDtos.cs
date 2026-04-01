using PersianHub.API.Enums.Layer1Hook;
using PersianHub.API.Enums.Common;

namespace PersianHub.API.DTOs.Layer1Hook;

public record CreateUserActivityDto(
    int AppUserId,
    ActivityType ActivityType,
    ReferenceType? ReferenceType = null,
    int? ReferenceId = null,
    string? Metadata = null
);

public record UserActivityDto(
    int Id,
    int AppUserId,
    ActivityType ActivityType,
    ReferenceType? ReferenceType,
    int? ReferenceId,
    string? Metadata,
    DateTime CreatedAtUtc
);

public record UserActivityListItemDto(
    int Id,
    ActivityType ActivityType,
    ReferenceType? ReferenceType,
    int? ReferenceId,
    DateTime CreatedAtUtc
);
