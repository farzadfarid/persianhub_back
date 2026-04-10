using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Common;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

/// <summary>
/// Reaction entity has no status field — moderation is hard delete only.
/// Hard delete is appropriate: reactions have no state, and removal is the only valid action.
/// </summary>
public sealed class AdminReactionService(ApplicationDbContext db) : IAdminReactionService
{
    public async Task<PagedResult<AdminReactionListItemDto>> GetAllAsync(
        int? userId, ReferenceType? referenceType, int? referenceId, ReactionType? reactionType,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.Reactions
            .AsNoTracking()
            .Include(r => r.AppUser)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(r => r.AppUserId == userId.Value);

        if (referenceType.HasValue)
            query = query.Where(r => r.ReferenceType == referenceType.Value);

        if (referenceId.HasValue)
            query = query.Where(r => r.ReferenceId == referenceId.Value);

        if (reactionType.HasValue)
            query = query.Where(r => r.ReactionType == reactionType.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(r => r.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new AdminReactionListItemDto(
                r.Id, r.AppUserId, r.AppUser.Email,
                r.ReferenceType, r.ReferenceId,
                r.ReactionType, r.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminReactionListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminReactionDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var reaction = await db.Reactions
            .AsNoTracking()
            .Include(r => r.AppUser)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (reaction is null)
            return Result<AdminReactionDetailDto>.Failure("Reaction not found.", ErrorCodes.NotFound);

        return Result<AdminReactionDetailDto>.Success(new AdminReactionDetailDto(
            reaction.Id, reaction.AppUserId, reaction.AppUser.Email,
            reaction.ReferenceType, reaction.ReferenceId,
            reaction.ReactionType, reaction.CreatedAtUtc));
    }

    public async Task<Result> RemoveAsync(int id, CancellationToken ct)
    {
        var reaction = await db.Reactions.FindAsync([id], ct);
        if (reaction is null)
            return Result.Failure("Reaction not found.", ErrorCodes.NotFound);

        db.Reactions.Remove(reaction);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
