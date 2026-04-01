using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.Entities.Layer2Core;

public class FeaturedPlacement
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public FeaturedPlacementType PlacementType { get; set; }
    public DateTime StartsAtUtc { get; set; }
    public DateTime EndsAtUtc { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    // Navigation
    public Business Business { get; set; } = null!;
}
