using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin visibility into the invite flow. Read-only.
/// Invites are user-initiated — admin does not create or modify them.
/// </summary>
[Route("api/v1/admin/invites")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminInvitesController(IAdminInviteService inviteService) : ApiControllerBase
{
    /// <summary>Paginated list of all invites with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? inviterUserId,
        [FromQuery] InviteStatus? status,
        [FromQuery] InviteChannel? channel,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await inviteService.GetAllAsync(inviterUserId, status, channel, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for an invite by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminInviteDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await inviteService.GetByIdAsync(id, ct));
}
