using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin visibility into referral flows.
/// No create/update/delete — referrals are system-generated when a referred user registers.
/// Admin may update status for edge case corrections only.
/// </summary>
[Route("api/v1/admin/referrals")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminReferralsController(IAdminReferralService referralService) : ApiControllerBase
{
    /// <summary>Paginated list of all referrals with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? referrerUserId,
        [FromQuery] int? referredUserId,
        [FromQuery] ReferralStatus? status,
        [FromQuery] RewardStatus? rewardStatus,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await referralService.GetAllAsync(
            referrerUserId, referredUserId, status, rewardStatus, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for a referral by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminReferralDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await referralService.GetByIdAsync(id, ct));

    /// <summary>Updates the status of a referral. Use for manual corrections only.</summary>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] AdminUpdateReferralStatusDto dto, CancellationToken ct)
        => MapResult(await referralService.UpdateStatusAsync(id, dto, ct));
}
