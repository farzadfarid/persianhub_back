using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin moderation of Reviews. Supports approve and reject status transitions.
/// </summary>
[Route("api/v1/admin/reviews")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminReviewsController(IAdminReviewService reviewService) : ApiControllerBase
{
    /// <summary>Paginated list of all reviews with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? businessId,
        [FromQuery] int? userId,
        [FromQuery] int? rating,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await reviewService.GetAllAsync(businessId, userId, rating, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for a review by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminReviewDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await reviewService.GetByIdAsync(id, ct));

    /// <summary>Approves a review — sets status to Approved.</summary>
    [HttpPatch("{id:int}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(int id, CancellationToken ct)
        => MapResult(await reviewService.ApproveAsync(id, ct));

    /// <summary>Rejects a review — sets status to Rejected.</summary>
    [HttpPatch("{id:int}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(int id, CancellationToken ct)
        => MapResult(await reviewService.RejectAsync(id, ct));
}
