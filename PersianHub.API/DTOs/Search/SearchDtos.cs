namespace PersianHub.API.DTOs.Search;

// ── Shared ────────────────────────────────────────────────────────────────────

/// <summary>Generic paginated result wrapper for all search endpoints.</summary>
public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize
);

// ── Business search ───────────────────────────────────────────────────────────

public record BusinessSearchRequestDto(
    string? Keyword,
    int? CategoryId,
    string? City,
    int Page = 1,
    int PageSize = 20
);

public record BusinessSearchItemDto(
    int Id,
    string Name,
    string? NameFa,
    string Slug,
    string? City,
    string? CityFa,
    string? PhoneNumber,
    string? LogoUrl,
    bool IsVerified,
    bool IsFeatured,
    double AverageRating,
    int ReviewCount
);

// ── Offer search ──────────────────────────────────────────────────────────────

public record OfferSearchRequestDto(
    string? Keyword,
    string? City,
    decimal? MinPrice,
    decimal? MaxPrice,
    int Page = 1,
    int PageSize = 20
);

public record OfferSearchItemDto(
    int Id,
    int BusinessId,
    string BusinessName,
    string BusinessSlug,
    string Title,
    string? TitleFa,
    string Slug,
    decimal? OriginalPrice,
    decimal? DiscountedPrice,
    decimal DiscountValue,
    string Currency,
    DateTime EndsAtUtc,
    string? CoverImageUrl
);

// ── Event search ──────────────────────────────────────────────────────────────

public record EventSearchRequestDto(
    string? Keyword,
    string? City,
    decimal? MinPrice,
    decimal? MaxPrice,
    bool? IsFree,
    int Page = 1,
    int PageSize = 20
);

public record EventSearchItemDto(
    int Id,
    string Title,
    string? TitleFa,
    string Slug,
    string? City,
    string? CityFa,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    bool IsFree,
    decimal? Price,
    string? Currency,
    string? CoverImageUrl
);
