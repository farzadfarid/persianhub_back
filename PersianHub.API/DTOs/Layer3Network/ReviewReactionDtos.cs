using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.DTOs.Layer3Network;

public record CreateReviewReactionDto(int ReviewId, int AppUserId, ReactionType ReactionType);

public record ReviewReactionDto(int Id, int ReviewId, int AppUserId, ReactionType ReactionType, DateTime CreatedAtUtc);
