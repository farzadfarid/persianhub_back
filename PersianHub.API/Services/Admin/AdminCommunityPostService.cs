using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Common;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

public sealed class AdminCommunityPostService(ApplicationDbContext db) : IAdminCommunityPostService
{
    public async Task<PagedResult<AdminCommunityPostListItemDto>> GetAllAsync(
        int? userId, ContentStatus? status, PostType? postType, string? search,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.CommunityPosts
            .AsNoTracking()
            .Include(p => p.AppUser)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(p => p.AppUserId == userId.Value);

        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        if (postType.HasValue)
            query = query.Where(p => p.PostType == postType.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p =>
                (p.Title != null && p.Title.Contains(search)) ||
                p.Body.Contains(search));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new AdminCommunityPostListItemDto(
                p.Id, p.AppUserId, p.AppUser.Email,
                p.PostType, p.Title, p.Status, p.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminCommunityPostListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminCommunityPostDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var post = await db.CommunityPosts
            .AsNoTracking()
            .Include(p => p.AppUser)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

        if (post is null)
            return Result<AdminCommunityPostDetailDto>.Failure("Community post not found.", ErrorCodes.NotFound);

        return Result<AdminCommunityPostDetailDto>.Success(new AdminCommunityPostDetailDto(
            post.Id, post.AppUserId, post.AppUser.Email,
            post.PostType, post.Title, post.Body, post.Status,
            post.CreatedAtUtc, post.UpdatedAtUtc));
    }

    public async Task<Result> ApproveAsync(int id, CancellationToken ct)
    {
        var post = await db.CommunityPosts.FindAsync([id], ct);
        if (post is null)
            return Result.Failure("Community post not found.", ErrorCodes.NotFound);

        post.Status = ContentStatus.Published;
        post.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> RejectAsync(int id, CancellationToken ct)
    {
        var post = await db.CommunityPosts.FindAsync([id], ct);
        if (post is null)
            return Result.Failure("Community post not found.", ErrorCodes.NotFound);

        post.Status = ContentStatus.Rejected;
        post.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> ArchiveAsync(int id, CancellationToken ct)
    {
        var post = await db.CommunityPosts.FindAsync([id], ct);
        if (post is null)
            return Result.Failure("Community post not found.", ErrorCodes.NotFound);

        post.Status = ContentStatus.Archived;
        post.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
