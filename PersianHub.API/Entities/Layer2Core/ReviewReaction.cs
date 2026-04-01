using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer2Core;

public class ReviewReaction
{
    public int Id { get; set; }
    public int ReviewId { get; set; }
    public int AppUserId { get; set; }
    public ReactionType ReactionType { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    // Navigation
    public Review Review { get; set; } = null!;
    public AppUser AppUser { get; set; } = null!;
}
