using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Enums.Common;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin moderation of community posts.
/// ContentStatus lifecycle: Draft → Published (approve) | Rejected | Archived.
/// No hard delete — use archive to remove from public view.
/// </summary>
[Route("api/v1/admin/community-posts")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminCommunityPostsController(IAdminCommunityPostService postService) : ApiControllerBase
{
    /// <summary>Paginated list of all community posts with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? userId,
        [FromQuery] ContentStatus? status,
        [FromQuery] PostType? postType,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await postService.GetAllAsync(userId, status, postType, search, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for a community post by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminCommunityPostDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await postService.GetByIdAsync(id, ct));

    /// <summary>Approves a post — sets ContentStatus to Published.</summary>
    [HttpPatch("{id:int}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(int id, CancellationToken ct)
        => MapResult(await postService.ApproveAsync(id, ct));

    /// <summary>Rejects a post — sets ContentStatus to Rejected.</summary>
    [HttpPatch("{id:int}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(int id, CancellationToken ct)
        => MapResult(await postService.RejectAsync(id, ct));

    /// <summary>Archives a post — sets ContentStatus to Archived. Use instead of delete.</summary>
    [HttpPatch("{id:int}/archive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Archive(int id, CancellationToken ct)
        => MapResult(await postService.ArchiveAsync(id, ct));
}
