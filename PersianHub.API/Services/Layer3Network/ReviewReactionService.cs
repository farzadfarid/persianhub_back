using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Services.Layer3Network;

public sealed class ReviewReactionService(ApplicationDbContext db) : IReviewReactionService
{
    public async Task<Result<ReviewReactionDto>> AddAsync(CreateReviewReactionDto request, CancellationToken ct = default)
    {
        var reviewExists = await db.Reviews.AnyAsync(r => r.Id == request.ReviewId, ct);
        if (!reviewExists)
            return Result<ReviewReactionDto>.Failure($"Review with id {request.ReviewId} not found.", ErrorCodes.NotFound);

        // One reaction per type per user per review
        var existing = await db.ReviewReactions.FirstOrDefaultAsync(
            rr => rr.ReviewId == request.ReviewId && rr.AppUserId == request.AppUserId && rr.ReactionType == request.ReactionType, ct);
        if (existing is not null)
            return Result<ReviewReactionDto>.Success(ToDto(existing));

        var entity = new ReviewReaction
        {
            ReviewId = request.ReviewId,
            AppUserId = request.AppUserId,
            ReactionType = request.ReactionType,
            CreatedAtUtc = DateTime.UtcNow,
        };

        db.ReviewReactions.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<ReviewReactionDto>.Success(ToDto(entity));
    }

    public async Task<Result> RemoveAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.ReviewReactions.FindAsync([id], ct);
        if (entity is null)
            return Result.Failure("Reaction not found.", ErrorCodes.NotFound);

        db.ReviewReactions.Remove(entity);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<ReviewReactionDto>>> GetByUserAsync(int appUserId, CancellationToken ct = default)
    {
        var items = await db.ReviewReactions
            .AsNoTracking()
            .Where(rr => rr.AppUserId == appUserId)
            .Select(rr => ToDto(rr))
            .ToListAsync(ct);

        return Result<IReadOnlyList<ReviewReactionDto>>.Success(items);
    }

    private static ReviewReactionDto ToDto(ReviewReaction rr) =>
        new(rr.Id, rr.ReviewId, rr.AppUserId, rr.ReactionType, rr.CreatedAtUtc);
}
