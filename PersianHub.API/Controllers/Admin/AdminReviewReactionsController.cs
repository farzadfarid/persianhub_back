using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin visibility into review reactions. No status field — hard delete is the only moderation action.
/// </summary>
[Route("api/v1/admin/review-reactions")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminReviewReactionsController(IAdminReviewReactionService reactionService) : ApiControllerBase
{
    /// <summary>Paginated list of all review reactions with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? userId,
        [FromQuery] int? reviewId,
        [FromQuery] ReactionType? reactionType,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await reactionService.GetAllAsync(userId, reviewId, reactionType, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for a review reaction by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminReviewReactionDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await reactionService.GetByIdAsync(id, ct));

    /// <summary>Removes a review reaction (hard delete). Use to remove spam or invalid reactions.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id, CancellationToken ct)
        => MapResult(await reactionService.RemoveAsync(id, ct));
}
