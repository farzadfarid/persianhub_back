using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

/// <summary>
/// Admin visibility into referral flows. No create/delete — referrals are system-generated.
/// </summary>
public sealed class AdminReferralService(ApplicationDbContext db) : IAdminReferralService
{
    public async Task<PagedResult<AdminReferralListItemDto>> GetAllAsync(
        int? referrerUserId, int? referredUserId,
        ReferralStatus? status, RewardStatus? rewardStatus,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.Referrals
            .AsNoTracking()
            .Include(r => r.ReferrerUser)
            .Include(r => r.ReferredUser)
            .Include(r => r.ReferralCode)
            .AsQueryable();

        if (referrerUserId.HasValue)
            query = query.Where(r => r.ReferrerUserId == referrerUserId.Value);

        if (referredUserId.HasValue)
            query = query.Where(r => r.ReferredUserId == referredUserId.Value);

        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        if (rewardStatus.HasValue)
            query = query.Where(r => r.RewardStatus == rewardStatus.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(r => r.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new AdminReferralListItemDto(
                r.Id,
                r.ReferrerUserId, r.ReferrerUser.Email,
                r.ReferredUserId, r.ReferredUser != null ? r.ReferredUser.Email : null,
                r.ReferralCodeId, r.ReferralCode != null ? r.ReferralCode.Code : null,
                r.Status, r.RewardStatus,
                r.CreatedAtUtc, r.CompletedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminReferralListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminReferralDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var referral = await db.Referrals
            .AsNoTracking()
            .Include(r => r.ReferrerUser)
            .Include(r => r.ReferredUser)
            .Include(r => r.ReferralCode)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (referral is null)
            return Result<AdminReferralDetailDto>.Failure("Referral not found.", ErrorCodes.NotFound);

        return Result<AdminReferralDetailDto>.Success(new AdminReferralDetailDto(
            referral.Id,
            referral.ReferrerUserId, referral.ReferrerUser.Email,
            referral.ReferredUserId, referral.ReferredUser?.Email,
            referral.ReferralCodeId, referral.ReferralCode?.Code,
            referral.Status, referral.RewardStatus,
            referral.CreatedAtUtc, referral.CompletedAtUtc, referral.UpdatedAtUtc));
    }

    public async Task<Result> UpdateStatusAsync(int id, AdminUpdateReferralStatusDto dto, CancellationToken ct)
    {
        var referral = await db.Referrals.FindAsync([id], ct);
        if (referral is null)
            return Result.Failure("Referral not found.", ErrorCodes.NotFound);

        referral.Status = dto.Status;
        if (dto.Status == ReferralStatus.Completed)
            referral.CompletedAtUtc = DateTime.UtcNow;

        referral.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
