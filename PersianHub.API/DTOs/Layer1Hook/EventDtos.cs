using PersianHub.API.Enums.Layer1Hook;

namespace PersianHub.API.DTOs.Layer1Hook;

public record CreateEventDto(
    string Title,
    string? Slug,
    string? Description,
    string? LocationName,
    string? AddressLine,
    string? City,
    string? Region,
    string? PostalCode,
    string? Country,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    int? BusinessId,
    string? OrganizerName,
    string? OrganizerPhoneNumber,
    string? OrganizerEmail,
    string? CoverImageUrl,
    bool IsFree,
    decimal? Price,
    string? Currency,
    int? CreatedByUserId
);

public record UpdateEventDto(
    string Title,
    string? Description,
    string? LocationName,
    string? AddressLine,
    string? City,
    string? Region,
    string? PostalCode,
    string? Country,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    int? BusinessId,
    string? OrganizerName,
    string? OrganizerPhoneNumber,
    string? OrganizerEmail,
    string? CoverImageUrl,
    bool IsFree,
    decimal? Price,
    string? Currency
);

public record EventDto(
    int Id,
    string Title,
    string Slug,
    string? Description,
    string? LocationName,
    string? AddressLine,
    string? City,
    string? Region,
    string? PostalCode,
    string? Country,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    int? BusinessId,
    string? OrganizerName,
    string? OrganizerPhoneNumber,
    string? OrganizerEmail,
    string? CoverImageUrl,
    bool IsFree,
    decimal? Price,
    string? Currency,
    EventStatus Status,
    bool IsPublished,
    int? CreatedByUserId,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);

public record EventListItemDto(
    int Id,
    string Title,
    string Slug,
    string? City,
    DateTime StartsAtUtc,
    bool IsFree,
    decimal? Price,
    string? Currency,
    EventStatus Status,
    bool IsPublished,
    string? CoverImageUrl,
    int? BusinessId
);
