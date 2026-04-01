using PersianHub.API.Common;
using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer2Core;

public class Review : AuditableEntity
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public int AppUserId { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

    // Navigation
    public Business Business { get; set; } = null!;
    public AppUser AppUser { get; set; } = null!;
    public ICollection<ReviewReaction> Reactions { get; set; } = [];
}
