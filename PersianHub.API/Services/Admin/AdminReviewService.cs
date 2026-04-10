using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

public sealed class AdminReviewService(ApplicationDbContext db) : IAdminReviewService
{
    public async Task<PagedResult<AdminReviewListItemDto>> GetAllAsync(
        int? businessId, int? userId, int? rating,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.Reviews
            .AsNoTracking()
            .Include(r => r.Business)
            .Include(r => r.AppUser)
            .AsQueryable();

        if (businessId.HasValue)
            query = query.Where(r => r.BusinessId == businessId.Value);

        if (userId.HasValue)
            query = query.Where(r => r.AppUserId == userId.Value);

        if (rating.HasValue)
            query = query.Where(r => r.Rating == rating.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(r => r.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new AdminReviewListItemDto(
                r.Id, r.BusinessId, r.Business.Name,
                r.AppUserId, r.AppUser.Email,
                r.Rating, r.Title, r.Status, r.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminReviewListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminReviewDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var review = await db.Reviews
            .AsNoTracking()
            .Include(r => r.Business)
            .Include(r => r.AppUser)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (review is null)
            return Result<AdminReviewDetailDto>.Failure("Review not found.", ErrorCodes.NotFound);

        return Result<AdminReviewDetailDto>.Success(new AdminReviewDetailDto(
            review.Id, review.BusinessId, review.Business.Name,
            review.AppUserId, review.AppUser.Email,
            review.Rating, review.Title, review.Comment,
            review.Status, review.CreatedAtUtc, review.UpdatedAtUtc));
    }

    public async Task<Result> ApproveAsync(int id, CancellationToken ct)
    {
        var review = await db.Reviews.FindAsync([id], ct);
        if (review is null)
            return Result.Failure("Review not found.", ErrorCodes.NotFound);

        review.Status = ReviewStatus.Approved;
        review.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> RejectAsync(int id, CancellationToken ct)
    {
        var review = await db.Reviews.FindAsync([id], ct);
        if (review is null)
            return Result.Failure("Review not found.", ErrorCodes.NotFound);

        review.Status = ReviewStatus.Rejected;
        review.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
