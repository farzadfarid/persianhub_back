using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Layer3Network;

public record CreateReviewDto(
    int BusinessId,
    int AppUserId,
    int Rating,
    string? Title,
    string? Comment
);

public record UpdateReviewDto(
    int Rating,
    string? Title,
    string? Comment
);

public record ReviewDto(
    int Id,
    int BusinessId,
    int AppUserId,
    int Rating,
    string? Title,
    string? Comment,
    ReviewStatus Status,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);

public record ReviewListItemDto(
    int Id,
    int BusinessId,
    int AppUserId,
    int Rating,
    string? Title,
    ReviewStatus Status,
    DateTime CreatedAtUtc
);
