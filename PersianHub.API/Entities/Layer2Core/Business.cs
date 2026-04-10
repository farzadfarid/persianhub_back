using PersianHub.API.Common;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer2Core;

public class Business : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameFa { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionFa { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? InstagramUrl { get; set; }
    public string? TelegramUrl { get; set; }
    public string? WhatsAppNumber { get; set; }
    public string? AddressLine { get; set; }
    public string? AddressLineFa { get; set; }
    public string? City { get; set; }
    public string? CityFa { get; set; }
    public string? Region { get; set; }
    public string? RegionFa { get; set; }
    public string? PostalCode { get; set; }
    public string Country { get; set; } = "Sweden";
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? LogoUrl { get; set; }
    public bool IsVerified { get; set; }
    public bool IsClaimed { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
    public int? OwnerUserId { get; set; }

    // Navigation
    public AppUser? OwnerUser { get; set; }
    public ICollection<BusinessImage> Images { get; set; } = [];
    public ICollection<BusinessWorkingHour> WorkingHours { get; set; } = [];
    public ICollection<BusinessCategory> Categories { get; set; } = [];
    public ICollection<BusinessTag> Tags { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<ContactRequest> ContactRequests { get; set; } = [];
    public ICollection<Subscription> Subscriptions { get; set; } = [];
    public ICollection<FeaturedPlacement> FeaturedPlacements { get; set; } = [];
    public ICollection<BusinessClaimRequest> ClaimRequests { get; set; } = [];
}
