using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer1Hook;

public class EventBookmark
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public int EventId { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    // Navigation
    public AppUser AppUser { get; set; } = null!;
    public Event Event { get; set; } = null!;
}
