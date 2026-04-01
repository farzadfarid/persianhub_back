using PersianHub.API.Common;
using PersianHub.API.Enums.Layer1Hook;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer1Hook;

public class SavedSearch : AuditableEntity
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public SavedSearchType SearchType { get; set; }
    public string? QueryText { get; set; }
    public string? FilterJson { get; set; }

    // Navigation
    public AppUser AppUser { get; set; } = null!;
}
