using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer3Network;

public class Referral
{
    public int Id { get; set; }
    public int ReferrerUserId { get; set; }
    public int? ReferredUserId { get; set; }
    public int? ReferralCodeId { get; set; }
    public ReferralStatus Status { get; set; } = ReferralStatus.Pending;
    public RewardStatus RewardStatus { get; set; } = RewardStatus.Pending;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    // Navigation
    public AppUser ReferrerUser { get; set; } = null!;
    public AppUser? ReferredUser { get; set; }
    public ReferralCode? ReferralCode { get; set; }
}
