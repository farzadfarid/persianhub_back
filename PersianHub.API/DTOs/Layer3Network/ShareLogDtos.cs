using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Enums.Common;

namespace PersianHub.API.DTOs.Layer3Network;

public record CreateShareLogDto(
    int? AppUserId,
    ShareType ShareType,
    ShareChannel Channel,
    ReferenceType ReferenceType,
    int ReferenceId
);

public record ShareLogDto(
    int Id,
    int? AppUserId,
    ShareType ShareType,
    ShareChannel Channel,
    ReferenceType ReferenceType,
    int ReferenceId,
    DateTime CreatedAtUtc
);

public record ShareLogListItemDto(
    int Id,
    int? AppUserId,
    ShareType ShareType,
    ShareChannel Channel,
    ReferenceType ReferenceType,
    int ReferenceId,
    DateTime CreatedAtUtc
);
