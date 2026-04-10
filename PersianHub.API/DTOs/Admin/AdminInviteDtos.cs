using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.DTOs.Admin;

public record AdminInviteListItemDto(
    int Id,
    int InviterUserId,
    string? InviterEmail,
    string? InviteeEmail,
    string? InviteePhoneNumber,
    InviteChannel Channel,
    InviteStatus Status,
    DateTime SentAtUtc,
    DateTime? AcceptedAtUtc
);

public record AdminInviteDetailDto(
    int Id,
    int InviterUserId,
    string? InviterEmail,
    string? InviteeEmail,
    string? InviteePhoneNumber,
    InviteChannel Channel,
    InviteStatus Status,
    DateTime SentAtUtc,
    DateTime? AcceptedAtUtc,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);
