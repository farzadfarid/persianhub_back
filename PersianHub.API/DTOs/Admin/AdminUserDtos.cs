namespace PersianHub.API.DTOs.Admin;

public record AdminUserListItemDto(
    int Id,
    string Email,
    string FirstName,
    string LastName,
    string Role,
    bool IsActive,
    DateTime CreatedAtUtc
);

public record AdminBusinessListItemDto(
    int Id,
    string Name,
    string? NameFa,
    string Slug,
    string? City,
    string? PhoneNumber,
    string? Email,
    string? LogoUrl,
    bool IsVerified,
    bool IsFeatured,
    bool IsActive,
    int? OwnerUserId,
    string? OwnerFirstName,
    string? OwnerLastName,
    string? OwnerEmail,
    DateTime CreatedAtUtc
);
