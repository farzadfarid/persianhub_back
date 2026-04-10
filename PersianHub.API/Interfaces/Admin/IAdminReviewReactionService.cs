using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.Interfaces.Admin;

/// <summary>
/// Admin visibility into review reactions. No status field exists — view + hard delete only.
/// </summary>
public interface IAdminReviewReactionService
{
    Task<PagedResult<AdminReviewReactionListItemDto>> GetAllAsync(
        int? userId, int? reviewId, ReactionType? reactionType,
        int page, int pageSize, CancellationToken ct);

    Task<Result<AdminReviewReactionDetailDto>> GetByIdAsync(int id, CancellationToken ct);

    Task<Result> RemoveAsync(int id, CancellationToken ct);
}
