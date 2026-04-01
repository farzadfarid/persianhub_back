using PersianHub.API.Common;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Enums.Common;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer3Network;

public class CommunityPost : AuditableEntity
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public string? Title { get; set; }
    public string Body { get; set; } = string.Empty;
    public PostType PostType { get; set; } = PostType.General;
    public ContentStatus Status { get; set; } = ContentStatus.Draft;

    // Navigation
    public AppUser AppUser { get; set; } = null!;
}
