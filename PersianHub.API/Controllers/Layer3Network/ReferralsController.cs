using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Controllers.Layer3Network;

/// <summary>
/// Manages user referrals for the growth engine.
/// Tracks who referred whom, referral status, and reward eligibility.
/// </summary>
[Route("api/v1/referrals")]
public sealed class ReferralsController(IReferralService referralService) : ApiControllerBase
{
    /// <summary>Creates a new referral record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReferralDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateReferralDto request, CancellationToken ct)
    {
        var result = await referralService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns a referral by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReferralDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await referralService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns all referrals created by a specific user (as referrer).</summary>
    [HttpGet("referrer/{userId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<ReferralListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByReferrer(int userId, CancellationToken ct)
    {
        var result = await referralService.GetByReferrerUserIdAsync(userId, ct);
        return MapResult(result);
    }

    /// <summary>Returns all referrals where a specific user was referred.</summary>
    [HttpGet("referred/{userId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<ReferralListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByReferred(int userId, CancellationToken ct)
    {
        var result = await referralService.GetByReferredUserIdAsync(userId, ct);
        return MapResult(result);
    }

    /// <summary>Updates the status and reward status of a referral.</summary>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateReferralStatusDto request, CancellationToken ct)
    {
        var result = await referralService.UpdateStatusAsync(id, request, ct);
        return MapResult(result);
    }
}
