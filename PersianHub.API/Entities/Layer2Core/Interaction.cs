using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer2Core;

public class Interaction
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public int? AppUserId { get; set; }
    public InteractionType InteractionType { get; set; }
    public int? ReferenceId { get; set; }
    public string? Metadata { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    // Navigation
    public Business Business { get; set; } = null!;
    public AppUser? AppUser { get; set; }
}
