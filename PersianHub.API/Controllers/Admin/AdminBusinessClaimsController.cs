using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin review of business ownership claim requests.
/// Approving a claim assigns Business.OwnerUserId to the claimant and promotes them to BusinessOwner if needed.
/// </summary>
[Route("api/v1/admin/business-claims")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminBusinessClaimsController(IAdminBusinessClaimService claimService) : ApiControllerBase
{
    /// <summary>Returns all business claim requests ordered by submission date.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BusinessClaimListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => MapResult(await claimService.GetAllAsync(ct));

    /// <summary>Returns full details for a business claim request.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BusinessClaimDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await claimService.GetByIdAsync(id, ct));

    /// <summary>
    /// Approves a claim request. Assigns Business.OwnerUserId to the claimant,
    /// sets IsClaimed = true, and promotes claimant to BusinessOwner if currently User.
    /// </summary>
    [HttpPatch("{id:int}/approve")]
    [ProducesResponseType(typeof(BusinessClaimDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Approve(int id, CancellationToken ct)
        => MapResult(await claimService.ApproveAsync(id, ct));

    /// <summary>Rejects a claim request. Status updated; no ownership changes.</summary>
    [HttpPatch("{id:int}/reject")]
    [ProducesResponseType(typeof(BusinessClaimDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Reject(int id, CancellationToken ct)
        => MapResult(await claimService.RejectAsync(id, ct));
}
