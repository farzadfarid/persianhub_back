using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.DTOs.Admin;

public record AdminReferralListItemDto(
    int Id,
    int ReferrerUserId,
    string? ReferrerEmail,
    int? ReferredUserId,
    string? ReferredEmail,
    int? ReferralCodeId,
    string? ReferralCode,
    ReferralStatus Status,
    RewardStatus RewardStatus,
    DateTime CreatedAtUtc,
    DateTime? CompletedAtUtc
);

public record AdminReferralDetailDto(
    int Id,
    int ReferrerUserId,
    string? ReferrerEmail,
    int? ReferredUserId,
    string? ReferredEmail,
    int? ReferralCodeId,
    string? ReferralCode,
    ReferralStatus Status,
    RewardStatus RewardStatus,
    DateTime CreatedAtUtc,
    DateTime? CompletedAtUtc,
    DateTime UpdatedAtUtc
);

public record AdminUpdateReferralStatusDto(ReferralStatus Status);
