using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer1Hook;

public class DealBookmark
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public int DealId { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    // Navigation
    public AppUser AppUser { get; set; } = null!;
    public Deal Deal { get; set; } = null!;
}
