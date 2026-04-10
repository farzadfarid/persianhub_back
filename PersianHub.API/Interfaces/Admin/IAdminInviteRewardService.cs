using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminInviteRewardService
{
    Task<PagedResult<AdminInviteRewardListItemDto>> GetAllAsync(
        int? userId, RewardStatus? status, RewardType? rewardType, int? referralId,
        int page, int pageSize, CancellationToken ct);

    Task<Result<AdminInviteRewardDetailDto>> GetByIdAsync(int id, CancellationToken ct);

    Task<Result> ApproveAsync(int id, CancellationToken ct);

    Task<Result> RejectAsync(int id, CancellationToken ct);

    Task<Result> MarkGrantedAsync(int id, CancellationToken ct);
}
