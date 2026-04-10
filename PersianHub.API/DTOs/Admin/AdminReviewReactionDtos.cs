using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.DTOs.Admin;

public record AdminReviewReactionListItemDto(
    int Id,
    int ReviewId,
    int AppUserId,
    string? UserEmail,
    ReactionType ReactionType,
    DateTime CreatedAtUtc
);

public record AdminReviewReactionDetailDto(
    int Id,
    int ReviewId,
    int AppUserId,
    string? UserEmail,
    ReactionType ReactionType,
    DateTime CreatedAtUtc
);
