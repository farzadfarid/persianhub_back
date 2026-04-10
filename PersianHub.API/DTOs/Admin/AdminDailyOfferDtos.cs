using PersianHub.API.Enums.Layer1Hook;

namespace PersianHub.API.DTOs.Admin;

public record AdminDailyOfferListItemDto(
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
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    bool IsActive,
    bool IsPublished,
    string? CoverImageUrl,
    DateTime CreatedAtUtc
);

public record AdminDailyOfferDetailDto(
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
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    bool IsActive,
    bool IsPublished,
    string? CoverImageUrl,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);

public record AdminCreateDailyOfferDto(
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
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    string? CoverImageUrl
);

public record AdminUpdateDailyOfferDto(
    string Title,
    string? TitleFa,
    string? Description,
    string? DescriptionFa,
    DiscountType DiscountType,
    decimal DiscountValue,
    decimal? OriginalPrice,
    decimal? DiscountedPrice,
    string Currency,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    string? CoverImageUrl
);
