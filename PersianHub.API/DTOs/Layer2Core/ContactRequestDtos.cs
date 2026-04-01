using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Layer2Core;

public record CreateContactRequestDto(
    int BusinessId,
    int? AppUserId,
    string Name,
    string Email,
    string? PhoneNumber,
    string? Message,
    ContactType ContactType
);

public record ContactRequestDto(
    int Id,
    int BusinessId,
    int? AppUserId,
    string Name,
    string Email,
    string? PhoneNumber,
    string? Message,
    ContactType ContactType,
    ContactRequestStatus Status,
    bool IsConverted,
    string? Metadata,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);

public record ContactRequestListItemDto(
    int Id,
    int BusinessId,
    int? AppUserId,
    string Name,
    ContactType ContactType,
    ContactRequestStatus Status,
    bool IsConverted,
    DateTime CreatedAtUtc
);
