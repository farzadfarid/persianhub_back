using PersianHub.API.Enums.Layer1Hook;

namespace PersianHub.API.DTOs.Layer1Hook;

public record CreateDailyOfferDto(
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

public record UpdateDailyOfferDto(
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

public record DailyOfferDto(
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
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    bool IsActive,
    bool IsPublished,
    string? CoverImageUrl,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);

public record DailyOfferListItemDto(
    int Id,
    int BusinessId,
    string Title,
    string? TitleFa,
    string Slug,
    DiscountType DiscountType,
    decimal DiscountValue,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    bool IsActive,
    bool IsPublished,
    string? CoverImageUrl
);
