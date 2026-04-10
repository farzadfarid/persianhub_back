using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Common;
using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.Interfaces.Admin;

/// <summary>
/// Admin visibility into reactions. No status field exists — view + hard delete only.
/// Hard delete is appropriate here: reactions have no state, and removal is the only valid moderation action.
/// </summary>
public interface IAdminReactionService
{
    Task<PagedResult<AdminReactionListItemDto>> GetAllAsync(
        int? userId, ReferenceType? referenceType, int? referenceId, ReactionType? reactionType,
        int page, int pageSize, CancellationToken ct);

    Task<Result<AdminReactionDetailDto>> GetByIdAsync(int id, CancellationToken ct);

    Task<Result> RemoveAsync(int id, CancellationToken ct);
}
