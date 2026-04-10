using PersianHub.API.Enums.Layer1Hook;

namespace PersianHub.API.DTOs.Layer1Hook;

public record CreateDealDto(
    int BusinessId,
    string Title,
    string? TitleFa,
    string? Slug,
    string? Description,
    string? DescriptionFa,
    DiscountType DiscountType,
    decimal DiscountValue,
    decimal? OriginalPrice,
    decimal? DiscountedPrice,
    string Currency,
    DateTime? ValidFromUtc,
    DateTime? ValidToUtc,
    string? CouponCode,
    string? TermsAndConditions,
    string? TermsAndConditionsFa,
    string? CoverImageUrl
);

public record UpdateDealDto(
    string Title,
    string? TitleFa,
    string? Description,
    string? DescriptionFa,
    DiscountType DiscountType,
    decimal DiscountValue,
    decimal? OriginalPrice,
    decimal? DiscountedPrice,
    string Currency,
    DateTime? ValidFromUtc,
    DateTime? ValidToUtc,
    string? CouponCode,
    string? TermsAndConditions,
    string? TermsAndConditionsFa,
    string? CoverImageUrl
);

public record DealDto(
    int Id,
    int BusinessId,
    string Title,
    string? TitleFa,
    string Slug,
    string? Description,
    string? DescriptionFa,
    DiscountType DiscountType,
    decimal DiscountValue,
    decimal? OriginalPrice,
    decimal? DiscountedPrice,
    string Currency,
    DateTime? ValidFromUtc,
    DateTime? ValidToUtc,
    string? CouponCode,
    string? TermsAndConditions,
    string? TermsAndConditionsFa,
    string? CoverImageUrl,
    bool IsPublished,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);

public record DealListItemDto(
    int Id,
    int BusinessId,
    string Title,
    string? TitleFa,
    string Slug,
    DiscountType DiscountType,
    decimal DiscountValue,
    DateTime? ValidFromUtc,
    DateTime? ValidToUtc,
    string? CouponCode,
    string? CoverImageUrl,
    bool IsPublished
);
