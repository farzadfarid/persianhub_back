using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer1Hook;

public class ListingBookmark
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public int MarketplaceListingId { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    // Navigation
    public AppUser AppUser { get; set; } = null!;
    public MarketplaceListing MarketplaceListing { get; set; } = null!;
}
