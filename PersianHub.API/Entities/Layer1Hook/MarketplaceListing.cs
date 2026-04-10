using PersianHub.API.Common;
using PersianHub.API.Enums.Layer1Hook;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer1Hook;

public class MarketplaceListing : AuditableEntity
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleFa { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionFa { get; set; }
    public MarketplaceListingType ListingType { get; set; }
    public string? Category { get; set; }
    public decimal? Price { get; set; }
    public string Currency { get; set; } = "SEK";
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Condition { get; set; }
    public string? ContactPhoneNumber { get; set; }
    public string? ContactEmail { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public MarketplaceListingStatus Status { get; set; } = MarketplaceListingStatus.Active;
    public DateTime? ExpiresAtUtc { get; set; }
    public bool IsPublished { get; set; }

    // Navigation
    public AppUser AppUser { get; set; } = null!;
    public ICollection<ListingBookmark> Bookmarks { get; set; } = [];
}
