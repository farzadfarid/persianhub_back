using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.Interfaces.Admin;

/// <summary>
/// Admin visibility into referral flows. No create/delete — referrals are system-generated.
/// </summary>
public interface IAdminReferralService
{
    Task<PagedResult<AdminReferralListItemDto>> GetAllAsync(
        int? referrerUserId, int? referredUserId,
        ReferralStatus? status, RewardStatus? rewardStatus,
        int page, int pageSize, CancellationToken ct);

    Task<Result<AdminReferralDetailDto>> GetByIdAsync(int id, CancellationToken ct);

    Task<Result> UpdateStatusAsync(int id, AdminUpdateReferralStatusDto dto, CancellationToken ct);
}
