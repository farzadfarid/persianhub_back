namespace PersianHub.API.DTOs.Layer2Core;

public record BusinessImageDto(
    int Id,
    string ImageUrl,
    string? AltText,
    int DisplayOrder,
    bool IsCover
);

public record CreateBusinessRequestDto(
    string Name,
    string? Slug,           // If omitted, generated from Name
    string? Description,
    string? PhoneNumber,
    string? Email,
    string? Website,
    string? InstagramUrl,
    string? TelegramUrl,
    string? WhatsAppNumber,
    string? AddressLine,
    string? City,
    string? Region,
    string? PostalCode,
    string Country,
    decimal? Latitude,
    decimal? Longitude,
    string? LogoUrl
    // OwnerUserId is intentionally omitted — set server-side from the authenticated user's token.
);

public record UpdateBusinessRequestDto(
    string Name,
    string? Description,
    string? PhoneNumber,
    string? Email,
    string? Website,
    string? InstagramUrl,
    string? TelegramUrl,
    string? WhatsAppNumber,
    string? AddressLine,
    string? City,
    string? Region,
    string? PostalCode,
    string Country,
    decimal? Latitude,
    decimal? Longitude,
    string? LogoUrl
);

public record BusinessDetailsDto(
    int Id,
    string Name,
    string Slug,
    string? LogoUrl,
    string? Description,
    string? PhoneNumber,
    string? Email,
    string? Website,
    string? InstagramUrl,
    string? TelegramUrl,
    string? WhatsAppNumber,
    string? AddressLine,
    string? City,
    string? Region,
    string? PostalCode,
    string Country,
    decimal? Latitude,
    decimal? Longitude,
    bool IsVerified,
    bool IsClaimed,
    bool IsFeatured,
    bool IsActive,
    int? OwnerUserId,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    IReadOnlyList<BusinessImageDto> Images
);

public record BusinessListItemDto(
    int Id,
    string Name,
    string Slug,
    string? City,
    string? PhoneNumber,
    string? LogoUrl,
    bool IsVerified,
    bool IsFeatured,
    bool IsActive
);

public record SetActiveStatusDto(bool IsActive);

public record AddBusinessImageDto(string ImageUrl, string? AltText, bool IsCover);
