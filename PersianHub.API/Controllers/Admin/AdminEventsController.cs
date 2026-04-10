using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Enums.Layer1Hook;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin management of Events. All operations are admin-only.
/// Does NOT reuse owner endpoints — queries all events across all businesses.
/// Delete sets Status = Cancelled (soft-delete equivalent for events).
/// </summary>
[Route("api/v1/admin/events")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminEventsController(IAdminEventService eventService) : ApiControllerBase
{
    /// <summary>Paginated list of all events with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? businessId,
        [FromQuery] EventStatus? status,
        [FromQuery] bool? isPublished,
        [FromQuery] DateTime? fromUtc,
        [FromQuery] DateTime? toUtc,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await eventService.GetAllAsync(businessId, status, isPublished, fromUtc, toUtc, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for an event by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminEventDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await eventService.GetByIdAsync(id, ct));

    /// <summary>Creates a new event for any business.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(AdminEventDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AdminCreateEventDto dto, CancellationToken ct)
    {
        var result = await eventService.CreateAsync(dto, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Updates an existing event.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(AdminEventDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] AdminUpdateEventDto dto, CancellationToken ct)
        => MapResult(await eventService.UpdateAsync(id, dto, ct));

    /// <summary>Toggles IsPublished on an event.</summary>
    [HttpPatch("{id:int}/toggle-active")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleActive(int id, CancellationToken ct)
        => MapResult(await eventService.TogglePublishedAsync(id, ct));

    /// <summary>Cancels an event — sets Status = Cancelled, IsPublished = false.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        => MapResult(await eventService.CancelAsync(id, ct));
}
