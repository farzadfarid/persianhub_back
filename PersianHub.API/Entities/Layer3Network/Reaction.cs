using PersianHub.API.Enums.Common;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer3Network;

public class Reaction
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public ReferenceType ReferenceType { get; set; }
    public int ReferenceId { get; set; }
    public ReactionType ReactionType { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    // Navigation
    public AppUser AppUser { get; set; } = null!;
}
