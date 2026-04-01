using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Admin;

public record BusinessClaimListItemDto(
    int Id,
    int BusinessId,
    string BusinessName,
    int AppUserId,
    BusinessClaimRequestStatus Status,
    DateTime CreatedAtUtc
);

public record BusinessClaimDetailDto(
    int Id,
    int BusinessId,
    string BusinessName,
    int AppUserId,
    string? SubmittedBusinessEmail,
    string? SubmittedPhoneNumber,
    string? Message,
    BusinessClaimRequestStatus Status,
    int? ReviewedByUserId,
    DateTime? ReviewedAtUtc,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);
