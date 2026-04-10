using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Enums.Common;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin visibility into reactions. Reaction has no status field — removal is the only moderation action.
/// Hard delete is used intentionally: reactions are atomic and have no state to transition.
/// </summary>
[Route("api/v1/admin/reactions")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminReactionsController(IAdminReactionService reactionService) : ApiControllerBase
{
    /// <summary>Paginated list of all reactions with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? userId,
        [FromQuery] ReferenceType? referenceType,
        [FromQuery] int? referenceId,
        [FromQuery] ReactionType? reactionType,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await reactionService.GetAllAsync(userId, referenceType, referenceId, reactionType, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for a reaction by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminReactionDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await reactionService.GetByIdAsync(id, ct));

    /// <summary>Removes a reaction (hard delete). Use to remove spam or invalid reactions.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id, CancellationToken ct)
        => MapResult(await reactionService.RemoveAsync(id, ct));
}
