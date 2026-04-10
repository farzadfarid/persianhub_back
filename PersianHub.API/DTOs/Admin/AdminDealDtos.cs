using PersianHub.API.Enums.Layer1Hook;

namespace PersianHub.API.DTOs.Admin;

public record AdminDealListItemDto(
    int Id,
    int BusinessId,
    string? BusinessName,
    string Title,
    string? TitleFa,
    string Slug,
    DiscountType DiscountType,
    decimal DiscountValue,
    decimal? OriginalPrice,
    decimal? DiscountedPrice,
    string Currency,
    DateTime? ValidFromUtc,
    DateTime? ValidToUtc,
    string? CouponCode,
    bool IsPublished,
    DateTime CreatedAtUtc
);

public record AdminDealDetailDto(
    int Id,
    int BusinessId,
    string? BusinessName,
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

public record AdminCreateDealDto(
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

public record AdminUpdateDealDto(
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
