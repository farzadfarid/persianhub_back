using PersianHub.API.Enums.Common;
using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.DTOs.Admin;

public record AdminCommunityPostListItemDto(
    int Id,
    int AppUserId,
    string? UserEmail,
    PostType PostType,
    string? Title,
    ContentStatus Status,
    DateTime CreatedAtUtc
);

public record AdminCommunityPostDetailDto(
    int Id,
    int AppUserId,
    string? UserEmail,
    PostType PostType,
    string? Title,
    string Body,
    ContentStatus Status,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);
