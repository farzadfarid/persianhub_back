using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.Interfaces.Admin;

/// <summary>
/// Admin visibility into the invite flow. Read-only — invites are user-initiated.
/// </summary>
public interface IAdminInviteService
{
    Task<PagedResult<AdminInviteListItemDto>> GetAllAsync(
        int? inviterUserId, InviteStatus? status, InviteChannel? channel,
        int page, int pageSize, CancellationToken ct);

    Task<Result<AdminInviteDetailDto>> GetByIdAsync(int id, CancellationToken ct);
}
