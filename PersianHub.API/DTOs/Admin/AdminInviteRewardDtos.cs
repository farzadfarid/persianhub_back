using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.DTOs.Admin;

public record AdminInviteRewardListItemDto(
    int Id,
    int AppUserId,
    string? UserEmail,
    int? ReferralId,
    RewardType RewardType,
    decimal RewardValue,
    string? Currency,
    RewardStatus Status,
    DateTime? GrantedAtUtc,
    DateTime CreatedAtUtc
);

public record AdminInviteRewardDetailDto(
    int Id,
    int AppUserId,
    string? UserEmail,
    int? ReferralId,
    RewardType RewardType,
    decimal RewardValue,
    string? Currency,
    RewardStatus Status,
    DateTime? GrantedAtUtc,
    DateTime CreatedAtUtc
);
