using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Admin;

public record AdminReviewListItemDto(
    int Id,
    int BusinessId,
    string? BusinessName,
    int AppUserId,
    string? UserEmail,
    int Rating,
    string? Title,
    ReviewStatus Status,
    DateTime CreatedAtUtc
);

public record AdminReviewDetailDto(
    int Id,
    int BusinessId,
    string? BusinessName,
    int AppUserId,
    string? UserEmail,
    int Rating,
    string? Title,
    string? Comment,
    ReviewStatus Status,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);
