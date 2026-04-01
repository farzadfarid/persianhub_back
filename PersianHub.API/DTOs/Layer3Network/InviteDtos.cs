using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.DTOs.Layer3Network;

public record CreateInviteDto(
    int InviterUserId,
    string? InviteeEmail,
    string? InviteePhoneNumber,
    InviteChannel Channel
);

public record InviteDto(
    int Id,
    int InviterUserId,
    string? InviteeEmail,
    string? InviteePhoneNumber,
    InviteChannel Channel,
    InviteStatus Status,
    DateTime SentAtUtc,
    DateTime? AcceptedAtUtc,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);

public record InviteListItemDto(
    int Id,
    int InviterUserId,
    string? InviteeEmail,
    string? InviteePhoneNumber,
    InviteChannel Channel,
    InviteStatus Status,
    DateTime CreatedAtUtc
);

public record UpdateInviteStatusDto(
    InviteStatus Status
);
