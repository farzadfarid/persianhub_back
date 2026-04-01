using PersianHub.API.Common;
using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer2Core;

public class BusinessClaimRequest : AuditableEntity
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public int AppUserId { get; set; }
    public string? SubmittedBusinessEmail { get; set; }
    public string? SubmittedPhoneNumber { get; set; }
    public string? Message { get; set; }
    public BusinessClaimRequestStatus Status { get; set; } = BusinessClaimRequestStatus.Pending;
    public int? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAtUtc { get; set; }

    // Navigation
    public Business Business { get; set; } = null!;
    public AppUser AppUser { get; set; } = null!;
    public AppUser? ReviewedByUser { get; set; }
}
