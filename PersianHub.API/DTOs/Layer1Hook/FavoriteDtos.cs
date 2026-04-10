using PersianHub.API.Enums.Common;

namespace PersianHub.API.DTOs.Layer1Hook;

public record AddFavoriteDto(
    int AppUserId,
    ReferenceType ReferenceType,
    int ReferenceId
);

public record FavoriteDto(
    int Id,
    int AppUserId,
    ReferenceType ReferenceType,
    int ReferenceId,
    DateTime CreatedAtUtc
);

public record FavoriteListItemDto(
    int Id,
    ReferenceType ReferenceType,
    int ReferenceId,
    DateTime CreatedAtUtc
);

public record BusinessFollowerDto(
    int AppUserId,
    string Name,
    string Email,
    DateTime SavedAtUtc
);

public record FavoriteCountDto(int Count);
