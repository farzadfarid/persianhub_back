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
