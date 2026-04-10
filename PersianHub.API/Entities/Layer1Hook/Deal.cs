using PersianHub.API.Common;
using PersianHub.API.Enums.Layer1Hook;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Entities.Layer1Hook;

public class Deal : AuditableEntity
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleFa { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionFa { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal? OriginalPrice { get; set; }
    public decimal? DiscountedPrice { get; set; }
    public string Currency { get; set; } = "SEK";
    public DateTime? ValidFromUtc { get; set; }
    public DateTime? ValidToUtc { get; set; }
    public string? CouponCode { get; set; }
    public string? TermsAndConditions { get; set; }
    public string? TermsAndConditionsFa { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsPublished { get; set; }

    // Navigation
    public Business Business { get; set; } = null!;
    public ICollection<DealBookmark> Bookmarks { get; set; } = [];
}
