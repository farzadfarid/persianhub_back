using PersianHub.API.Common;
using PersianHub.API.Enums.Common;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer3Network;

public class Comment : AuditableEntity
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public ReferenceType ReferenceType { get; set; }
    public int ReferenceId { get; set; }
    public string Body { get; set; } = string.Empty;
    public ContentStatus Status { get; set; } = ContentStatus.Published;

    // Navigation
    public AppUser AppUser { get; set; } = null!;
}
