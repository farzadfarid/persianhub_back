using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer3Network;

public class BusinessSuggestion
{
    public int Id { get; set; }
    public int? SuggestedByUserId { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public string? CategoryText { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? AddressLine { get; set; }
    public string? City { get; set; }
    public string? Description { get; set; }
    public BusinessClaimRequestStatus Status { get; set; } = BusinessClaimRequestStatus.Pending;
    public int? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    // Navigation
    public AppUser? SuggestedByUser { get; set; }
    public AppUser? ReviewedByUser { get; set; }
}
