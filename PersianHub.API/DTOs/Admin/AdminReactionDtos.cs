using PersianHub.API.Enums.Common;
using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.DTOs.Admin;

public record AdminReactionListItemDto(
    int Id,
    int AppUserId,
    string? UserEmail,
    ReferenceType ReferenceType,
    int ReferenceId,
    ReactionType ReactionType,
    DateTime CreatedAtUtc
);

public record AdminReactionDetailDto(
    int Id,
    int AppUserId,
    string? UserEmail,
    ReferenceType ReferenceType,
    int ReferenceId,
    ReactionType ReactionType,
    DateTime CreatedAtUtc
);
