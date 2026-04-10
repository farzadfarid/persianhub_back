using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Enums.Common;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin moderation of comments.
/// ContentStatus lifecycle: Published (default) → Rejected | Archived.
/// No hard delete — use archive or reject to suppress content.
/// </summary>
[Route("api/v1/admin/comments")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminCommentsController(IAdminCommentService commentService) : ApiControllerBase
{
    /// <summary>Paginated list of all comments with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? userId,
        [FromQuery] ContentStatus? status,
        [FromQuery] ReferenceType? referenceType,
        [FromQuery] int? referenceId,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await commentService.GetAllAsync(userId, status, referenceType, referenceId, search, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for a comment by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminCommentDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await commentService.GetByIdAsync(id, ct));

    /// <summary>Approves a comment — sets ContentStatus to Published.</summary>
    [HttpPatch("{id:int}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(int id, CancellationToken ct)
        => MapResult(await commentService.ApproveAsync(id, ct));

    /// <summary>Rejects a comment — sets ContentStatus to Rejected.</summary>
    [HttpPatch("{id:int}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(int id, CancellationToken ct)
        => MapResult(await commentService.RejectAsync(id, ct));

    /// <summary>Archives a comment — sets ContentStatus to Archived. Use instead of delete.</summary>
    [HttpPatch("{id:int}/archive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Archive(int id, CancellationToken ct)
        => MapResult(await commentService.ArchiveAsync(id, ct));
}
