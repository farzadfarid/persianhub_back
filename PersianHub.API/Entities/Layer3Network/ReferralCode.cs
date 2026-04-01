using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer3Network;

public class ReferralCode
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    // Navigation
    public AppUser AppUser { get; set; } = null!;
    public ICollection<Referral> Referrals { get; set; } = [];
}
