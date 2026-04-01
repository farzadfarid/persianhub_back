using PersianHub.API.Enums.Common;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer1Hook;

public class Favorite
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public ReferenceType ReferenceType { get; set; }
    public int ReferenceId { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    // Navigation
    public AppUser AppUser { get; set; } = null!;
}
