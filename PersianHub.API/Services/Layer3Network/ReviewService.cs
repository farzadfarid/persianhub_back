using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Services.Layer3Network;

public sealed class ReviewService(ApplicationDbContext db, IDateTimeProvider clock) : IReviewService
{
    public async Task<Result<ReviewDto>> CreateAsync(CreateReviewDto request, CancellationToken ct = default)
    {
        if (request.Rating is < 1 or > 5)
            return Result<ReviewDto>.Failure("Rating must be between 1 and 5.", ErrorCodes.ValidationFailed);

        var businessExists = await db.Businesses.AnyAsync(b => b.Id == request.BusinessId, ct);
        if (!businessExists)
            return Result<ReviewDto>.Failure($"Business with id {request.BusinessId} not found.", ErrorCodes.NotFound);

        var userExists = await db.AppUsers.AnyAsync(u => u.Id == request.AppUserId, ct);
        if (!userExists)
            return Result<ReviewDto>.Failure($"User with id {request.AppUserId} not found.", ErrorCodes.NotFound);

        // One review per user per business
        var existing = await db.Reviews.AnyAsync(
            r => r.BusinessId == request.BusinessId && r.AppUserId == request.AppUserId, ct);
        if (existing)
            return Result<ReviewDto>.Failure("User has already reviewed this business.", ErrorCodes.Conflict);

        var now = clock.UtcNow;
        var entity = new Review
        {
            BusinessId = request.BusinessId,
            AppUserId = request.AppUserId,
            Rating = request.Rating,
            Title = request.Title?.Trim(),
            Comment = request.Comment?.Trim(),
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.Reviews.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<ReviewDto>.Success(ToDto(entity));
    }

    public async Task<Result<ReviewDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Reviews.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);
        if (entity is null)
            return Result<ReviewDto>.Failure($"Review with id {id} not found.", ErrorCodes.NotFound);

        return Result<ReviewDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<ReviewListItemDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default)
    {
        var businessExists = await db.Businesses.AnyAsync(b => b.Id == businessId, ct);
        if (!businessExists)
            return Result<IReadOnlyList<ReviewListItemDto>>.Failure($"Business with id {businessId} not found.", ErrorCodes.NotFound);

        var items = await db.Reviews
            .AsNoTracking()
            .Where(r => r.BusinessId == businessId)
            .OrderByDescending(r => r.CreatedAtUtc)
            .Select(r => new ReviewListItemDto(r.Id, r.BusinessId, r.AppUserId, r.Rating, r.Title, r.Status, r.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<ReviewListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<ReviewListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == appUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<ReviewListItemDto>>.Failure($"User with id {appUserId} not found.", ErrorCodes.NotFound);

        var items = await db.Reviews
            .AsNoTracking()
            .Where(r => r.AppUserId == appUserId)
            .OrderByDescending(r => r.CreatedAtUtc)
            .Select(r => new ReviewListItemDto(r.Id, r.BusinessId, r.AppUserId, r.Rating, r.Title, r.Status, r.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<ReviewListItemDto>>.Success(items);
    }

    public async Task<Result<ReviewDto>> UpdateAsync(int id, UpdateReviewDto request, CancellationToken ct = default)
    {
        if (request.Rating is < 1 or > 5)
            return Result<ReviewDto>.Failure("Rating must be between 1 and 5.", ErrorCodes.ValidationFailed);

        var entity = await db.Reviews.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (entity is null)
            return Result<ReviewDto>.Failure($"Review with id {id} not found.", ErrorCodes.NotFound);

        entity.Rating = request.Rating;
        entity.Title = request.Title?.Trim();
        entity.Comment = request.Comment?.Trim();
        entity.UpdatedAtUtc = clock.UtcNow;

        await db.SaveChangesAsync(ct);

        return Result<ReviewDto>.Success(ToDto(entity));
    }

    private static ReviewDto ToDto(Review r) => new(
        r.Id, r.BusinessId, r.AppUserId, r.Rating, r.Title, r.Comment,
        r.Status, r.CreatedAtUtc, r.UpdatedAtUtc);
}
