using PersianHub.API.Common;
using PersianHub.API.Enums.Layer1Hook;
using PersianHub.API.Entities.Common;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Entities.Layer1Hook;

public class Event : AuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleFa { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionFa { get; set; }
    public string? LocationName { get; set; }
    public string? LocationNameFa { get; set; }
    public string? AddressLine { get; set; }
    public string? AddressLineFa { get; set; }
    public string? City { get; set; }
    public string? CityFa { get; set; }
    public string? Region { get; set; }
    public string? RegionFa { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public DateTime StartsAtUtc { get; set; }
    public DateTime? EndsAtUtc { get; set; }
    public string? OrganizerName { get; set; }
    public string? OrganizerNameFa { get; set; }
    public string? OrganizerPhoneNumber { get; set; }
    public string? OrganizerEmail { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsFree { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Draft;
    public bool IsPublished { get; set; }
    public int? CreatedByUserId { get; set; }
    public int? BusinessId { get; set; }

    // Navigation
    public AppUser? CreatedByUser { get; set; }
    public Business? Business { get; set; }
    public ICollection<EventBookmark> Bookmarks { get; set; } = [];
}
