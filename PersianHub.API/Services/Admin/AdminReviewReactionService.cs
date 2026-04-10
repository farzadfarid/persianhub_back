using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

/// <summary>
/// ReviewReaction entity has no status field — moderation is hard delete only.
/// </summary>
public sealed class AdminReviewReactionService(ApplicationDbContext db) : IAdminReviewReactionService
{
    public async Task<PagedResult<AdminReviewReactionListItemDto>> GetAllAsync(
        int? userId, int? reviewId, ReactionType? reactionType,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.ReviewReactions
            .AsNoTracking()
            .Include(rr => rr.AppUser)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(rr => rr.AppUserId == userId.Value);

        if (reviewId.HasValue)
            query = query.Where(rr => rr.ReviewId == reviewId.Value);

        if (reactionType.HasValue)
            query = query.Where(rr => rr.ReactionType == reactionType.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(rr => rr.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(rr => new AdminReviewReactionListItemDto(
                rr.Id, rr.ReviewId, rr.AppUserId, rr.AppUser.Email,
                rr.ReactionType, rr.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminReviewReactionListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminReviewReactionDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var rr = await db.ReviewReactions
            .AsNoTracking()
            .Include(r => r.AppUser)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (rr is null)
            return Result<AdminReviewReactionDetailDto>.Failure("Review reaction not found.", ErrorCodes.NotFound);

        return Result<AdminReviewReactionDetailDto>.Success(new AdminReviewReactionDetailDto(
            rr.Id, rr.ReviewId, rr.AppUserId, rr.AppUser.Email,
            rr.ReactionType, rr.CreatedAtUtc));
    }

    public async Task<Result> RemoveAsync(int id, CancellationToken ct)
    {
        var rr = await db.ReviewReactions.FindAsync([id], ct);
        if (rr is null)
            return Result.Failure("Review reaction not found.", ErrorCodes.NotFound);

        db.ReviewReactions.Remove(rr);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
