using PersianHub.API.Enums.Common;

namespace PersianHub.API.DTOs.Admin;

public record AdminCommentListItemDto(
    int Id,
    int AppUserId,
    string? UserEmail,
    ReferenceType ReferenceType,
    int ReferenceId,
    string Body,
    ContentStatus Status,
    DateTime CreatedAtUtc
);

public record AdminCommentDetailDto(
    int Id,
    int AppUserId,
    string? UserEmail,
    ReferenceType ReferenceType,
    int ReferenceId,
    string Body,
    ContentStatus Status,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);
