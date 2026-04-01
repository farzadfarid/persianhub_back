using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.DTOs.Layer3Network;

public record CreateReferralDto(
    int ReferrerUserId,
    int? ReferredUserId,
    int? ReferralCodeId
);

public record ReferralDto(
    int Id,
    int ReferrerUserId,
    int? ReferredUserId,
    int? ReferralCodeId,
    ReferralStatus Status,
    RewardStatus RewardStatus,
    DateTime CreatedAtUtc,
    DateTime? CompletedAtUtc,
    DateTime UpdatedAtUtc
);

public record ReferralListItemDto(
    int Id,
    int ReferrerUserId,
    int? ReferredUserId,
    ReferralStatus Status,
    RewardStatus RewardStatus,
    DateTime CreatedAtUtc
);

public record UpdateReferralStatusDto(
    ReferralStatus Status,
    RewardStatus RewardStatus
);
