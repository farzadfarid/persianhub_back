using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Enums.Common;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer3Network;

public class ShareLog
{
    public int Id { get; set; }
    public int? AppUserId { get; set; }
    public ShareType ShareType { get; set; }
    public ShareChannel Channel { get; set; }
    public ReferenceType ReferenceType { get; set; }
    public int ReferenceId { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    // Navigation
    public AppUser? AppUser { get; set; }
}
