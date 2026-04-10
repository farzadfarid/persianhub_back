namespace PersianHub.API.DTOs.Admin;

public record AdminReferralCodeListItemDto(
    int Id,
    int AppUserId,
    string? UserEmail,
    string Code,
    bool IsActive,
    int ReferralCount,
    DateTime CreatedAtUtc
);

public record AdminReferralCodeDetailDto(
    int Id,
    int AppUserId,
    string? UserEmail,
    string Code,
    bool IsActive,
    int ReferralCount,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);

/// <summary>Admin can create a referral code and assign it to any user.</summary>
public record AdminCreateReferralCodeDto(
    int AppUserId,
    string Code
);
