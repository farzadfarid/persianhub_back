using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer3Network;

public class InviteReward
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public int? ReferralId { get; set; }
    public RewardType RewardType { get; set; }
    public decimal RewardValue { get; set; }
    public string? Currency { get; set; }
    public RewardStatus Status { get; set; } = RewardStatus.Pending;
    public DateTime? GrantedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    // Navigation
    public AppUser AppUser { get; set; } = null!;
    public Referral? Referral { get; set; }
}
