using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer3Network;

public class Invite
{
    public int Id { get; set; }
    public int InviterUserId { get; set; }
    public string? InviteeEmail { get; set; }
    public string? InviteePhoneNumber { get; set; }
    public InviteChannel Channel { get; set; }
    public InviteStatus Status { get; set; } = InviteStatus.Sent;
    public DateTime SentAtUtc { get; set; }
    public DateTime? AcceptedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    // Navigation
    public AppUser InviterUser { get; set; } = null!;
}
