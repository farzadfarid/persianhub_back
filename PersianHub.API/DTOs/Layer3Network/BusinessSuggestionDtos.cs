using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Layer3Network;

public record CreateBusinessSuggestionDto(
    int? SuggestedByUserId,
    string BusinessName,
    string? CategoryText,
    string? PhoneNumber,
    string? Email,
    string? Website,
    string? AddressLine,
    string? City,
    string? Description
);

public record BusinessSuggestionDto(
    int Id,
    int? SuggestedByUserId,
    string BusinessName,
    string? CategoryText,
    string? PhoneNumber,
    string? Email,
    string? Website,
    string? AddressLine,
    string? City,
    string? Description,
    BusinessClaimRequestStatus Status,
    int? ReviewedByUserId,
    DateTime? ReviewedAtUtc,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);

public record BusinessSuggestionListItemDto(
    int Id,
    int? SuggestedByUserId,
    string BusinessName,
    BusinessClaimRequestStatus Status,
    DateTime CreatedAtUtc
);

public record UpdateBusinessSuggestionStatusDto(
    BusinessClaimRequestStatus Status,
    int? ReviewedByUserId
);
