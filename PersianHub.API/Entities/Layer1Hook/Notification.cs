using PersianHub.API.Enums.Layer1Hook;
using PersianHub.API.Enums.Common;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer1Hook;

public class Notification
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public ReferenceType? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    // Navigation
    public AppUser AppUser { get; set; } = null!;
}
