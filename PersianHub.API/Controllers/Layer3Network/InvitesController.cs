using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Controllers.Layer3Network;

/// <summary>
/// Manages user invites for the growth engine.
/// Invites can be sent via email or phone; at least one must be provided.
/// </summary>
[Route("api/v1/invites")]
public sealed class InvitesController(IInviteService inviteService) : ApiControllerBase
{
    /// <summary>Creates a new invite sent by a user.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(InviteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateInviteDto request, CancellationToken ct)
    {
        var result = await inviteService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns an invite by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InviteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await inviteService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns all invites sent by a specific user.</summary>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<InviteListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUser(int userId, CancellationToken ct)
    {
        var result = await inviteService.GetBySenderUserIdAsync(userId, ct);
        return MapResult(result);
    }

    /// <summary>Updates the status of an invite (e.g., mark as accepted).</summary>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateInviteStatusDto request, CancellationToken ct)
    {
        var result = await inviteService.UpdateStatusAsync(id, request, ct);
        return MapResult(result);
    }
}
