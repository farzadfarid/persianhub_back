using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Common;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

public sealed class AdminCommentService(ApplicationDbContext db) : IAdminCommentService
{
    public async Task<PagedResult<AdminCommentListItemDto>> GetAllAsync(
        int? userId, ContentStatus? status, ReferenceType? referenceType, int? referenceId, string? search,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.Comments
            .AsNoTracking()
            .Include(c => c.AppUser)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(c => c.AppUserId == userId.Value);

        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        if (referenceType.HasValue)
            query = query.Where(c => c.ReferenceType == referenceType.Value);

        if (referenceId.HasValue)
            query = query.Where(c => c.ReferenceId == referenceId.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(c => c.Body.Contains(search));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(c => c.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new AdminCommentListItemDto(
                c.Id, c.AppUserId, c.AppUser.Email,
                c.ReferenceType, c.ReferenceId,
                c.Body, c.Status, c.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminCommentListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminCommentDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var comment = await db.Comments
            .AsNoTracking()
            .Include(c => c.AppUser)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (comment is null)
            return Result<AdminCommentDetailDto>.Failure("Comment not found.", ErrorCodes.NotFound);

        return Result<AdminCommentDetailDto>.Success(new AdminCommentDetailDto(
            comment.Id, comment.AppUserId, comment.AppUser.Email,
            comment.ReferenceType, comment.ReferenceId,
            comment.Body, comment.Status,
            comment.CreatedAtUtc, comment.UpdatedAtUtc));
    }

    public async Task<Result> ApproveAsync(int id, CancellationToken ct)
    {
        var comment = await db.Comments.FindAsync([id], ct);
        if (comment is null)
            return Result.Failure("Comment not found.", ErrorCodes.NotFound);

        comment.Status = ContentStatus.Published;
        comment.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> RejectAsync(int id, CancellationToken ct)
    {
        var comment = await db.Comments.FindAsync([id], ct);
        if (comment is null)
            return Result.Failure("Comment not found.", ErrorCodes.NotFound);

        comment.Status = ContentStatus.Rejected;
        comment.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> ArchiveAsync(int id, CancellationToken ct)
    {
        var comment = await db.Comments.FindAsync([id], ct);
        if (comment is null)
            return Result.Failure("Comment not found.", ErrorCodes.NotFound);

        comment.Status = ContentStatus.Archived;
        comment.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
