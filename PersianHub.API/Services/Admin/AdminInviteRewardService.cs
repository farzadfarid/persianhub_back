using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

public sealed class AdminInviteRewardService(ApplicationDbContext db) : IAdminInviteRewardService
{
    public async Task<PagedResult<AdminInviteRewardListItemDto>> GetAllAsync(
        int? userId, RewardStatus? status, RewardType? rewardType, int? referralId,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.InviteRewards
            .AsNoTracking()
            .Include(ir => ir.AppUser)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(ir => ir.AppUserId == userId.Value);

        if (status.HasValue)
            query = query.Where(ir => ir.Status == status.Value);

        if (rewardType.HasValue)
            query = query.Where(ir => ir.RewardType == rewardType.Value);

        if (referralId.HasValue)
            query = query.Where(ir => ir.ReferralId == referralId.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(ir => ir.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ir => new AdminInviteRewardListItemDto(
                ir.Id, ir.AppUserId, ir.AppUser.Email,
                ir.ReferralId, ir.RewardType, ir.RewardValue, ir.Currency,
                ir.Status, ir.GrantedAtUtc, ir.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminInviteRewardListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminInviteRewardDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var reward = await db.InviteRewards
            .AsNoTracking()
            .Include(ir => ir.AppUser)
            .FirstOrDefaultAsync(ir => ir.Id == id, ct);

        if (reward is null)
            return Result<AdminInviteRewardDetailDto>.Failure("Invite reward not found.", ErrorCodes.NotFound);

        return Result<AdminInviteRewardDetailDto>.Success(new AdminInviteRewardDetailDto(
            reward.Id, reward.AppUserId, reward.AppUser.Email,
            reward.ReferralId, reward.RewardType, reward.RewardValue, reward.Currency,
            reward.Status, reward.GrantedAtUtc, reward.CreatedAtUtc));
    }

    public async Task<Result> ApproveAsync(int id, CancellationToken ct)
    {
        var reward = await db.InviteRewards.FindAsync([id], ct);
        if (reward is null)
            return Result.Failure("Invite reward not found.", ErrorCodes.NotFound);

        if (reward.Status != RewardStatus.Pending)
            return Result.Failure("Only pending rewards can be approved.", ErrorCodes.ValidationFailed);

        reward.Status = RewardStatus.Granted;
        reward.GrantedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> RejectAsync(int id, CancellationToken ct)
    {
        var reward = await db.InviteRewards.FindAsync([id], ct);
        if (reward is null)
            return Result.Failure("Invite reward not found.", ErrorCodes.NotFound);

        if (reward.Status != RewardStatus.Pending)
            return Result.Failure("Only pending rewards can be rejected.", ErrorCodes.ValidationFailed);

        reward.Status = RewardStatus.Expired;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> MarkGrantedAsync(int id, CancellationToken ct)
    {
        var reward = await db.InviteRewards.FindAsync([id], ct);
        if (reward is null)
            return Result.Failure("Invite reward not found.", ErrorCodes.NotFound);

        reward.Status = RewardStatus.Granted;
        reward.GrantedAtUtc ??= DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
