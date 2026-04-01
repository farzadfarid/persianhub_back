using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Layer2Core;

public record CreateInteractionDto(
    int BusinessId,
    int? AppUserId,
    InteractionType InteractionType,
    int? ReferenceId,
    string? Metadata
);

public record InteractionDto(
    int Id,
    int BusinessId,
    int? AppUserId,
    InteractionType InteractionType,
    int? ReferenceId,
    string? Metadata,
    DateTime CreatedAtUtc
);

public record InteractionListItemDto(
    int Id,
    int BusinessId,
    int? AppUserId,
    InteractionType InteractionType,
    DateTime CreatedAtUtc
);

public record InteractionCountsDto(
    int BusinessId,
    int TotalViews,
    int TotalClicks,
    int TotalContactEvents
);
