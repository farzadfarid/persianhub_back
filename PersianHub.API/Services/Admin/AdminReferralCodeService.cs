using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Entities.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

public sealed class AdminReferralCodeService(ApplicationDbContext db) : IAdminReferralCodeService
{
    public async Task<PagedResult<AdminReferralCodeListItemDto>> GetAllAsync(
        int? userId, bool? isActive, string? search,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.ReferralCodes
            .AsNoTracking()
            .Include(rc => rc.AppUser)
            .Include(rc => rc.Referrals)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(rc => rc.AppUserId == userId.Value);

        if (isActive.HasValue)
            query = query.Where(rc => rc.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(rc => rc.Code.Contains(search));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(rc => rc.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(rc => new AdminReferralCodeListItemDto(
                rc.Id, rc.AppUserId, rc.AppUser.Email,
                rc.Code, rc.IsActive,
                rc.Referrals.Count,
                rc.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminReferralCodeListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminReferralCodeDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var rc = await db.ReferralCodes
            .AsNoTracking()
            .Include(r => r.AppUser)
            .Include(r => r.Referrals)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (rc is null)
            return Result<AdminReferralCodeDetailDto>.Failure("Referral code not found.", ErrorCodes.NotFound);

        return Result<AdminReferralCodeDetailDto>.Success(new AdminReferralCodeDetailDto(
            rc.Id, rc.AppUserId, rc.AppUser.Email,
            rc.Code, rc.IsActive,
            rc.Referrals.Count,
            rc.CreatedAtUtc, rc.UpdatedAtUtc));
    }

    public async Task<Result<AdminReferralCodeDetailDto>> CreateAsync(AdminCreateReferralCodeDto dto, CancellationToken ct)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == dto.AppUserId, ct);
        if (!userExists)
            return Result<AdminReferralCodeDetailDto>.Failure("User not found.", ErrorCodes.NotFound);

        var codeExists = await db.ReferralCodes.AnyAsync(rc => rc.Code == dto.Code, ct);
        if (codeExists)
            return Result<AdminReferralCodeDetailDto>.Failure("Referral code already exists.", ErrorCodes.AlreadyExists);

        var now = DateTime.UtcNow;
        var rc = new ReferralCode
        {
            AppUserId = dto.AppUserId,
            Code = dto.Code,
            IsActive = true,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
        };

        db.ReferralCodes.Add(rc);
        await db.SaveChangesAsync(ct);

        var email = await db.AppUsers
            .Where(u => u.Id == dto.AppUserId)
            .Select(u => u.Email)
            .FirstOrDefaultAsync(ct);

        return Result<AdminReferralCodeDetailDto>.Success(new AdminReferralCodeDetailDto(
            rc.Id, rc.AppUserId, email, rc.Code, rc.IsActive, 0, rc.CreatedAtUtc, rc.UpdatedAtUtc));
    }

    public async Task<Result> ToggleActiveAsync(int id, CancellationToken ct)
    {
        var rc = await db.ReferralCodes.FindAsync([id], ct);
        if (rc is null)
            return Result.Failure("Referral code not found.", ErrorCodes.NotFound);

        rc.IsActive = !rc.IsActive;
        rc.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
