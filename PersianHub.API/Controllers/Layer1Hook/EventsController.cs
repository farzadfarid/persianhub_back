using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Controllers.Layer1Hook;

/// <summary>
/// Manages community events. Events may be linked to a business or standalone.
/// Supports full lifecycle: create, update, publish/unpublish.
/// </summary>
[Route("api/v1/events")]
public sealed class EventsController(IEventService eventService) : ApiControllerBase
{
    /// <summary>Creates a new event.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateEventDto request, CancellationToken ct)
    {
        var result = await eventService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns all events.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EventListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await eventService.GetAllAsync(ct);
        return MapResult(result);
    }

    /// <summary>Returns an event by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await eventService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns an event by its slug.</summary>
    [HttpGet("slug/{slug}")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug(string slug, CancellationToken ct)
    {
        var result = await eventService.GetBySlugAsync(slug, ct);
        return MapResult(result);
    }

    /// <summary>Returns all published events.</summary>
    [HttpGet("published")]
    [ProducesResponseType(typeof(IReadOnlyList<EventListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPublished(CancellationToken ct)
    {
        var result = await eventService.GetPublishedAsync(ct);
        return MapResult(result);
    }

    /// <summary>Returns all events linked to a specific business.</summary>
    [HttpGet("business/{businessId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<EventListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByBusiness(int businessId, CancellationToken ct)
    {
        var result = await eventService.GetByBusinessIdAsync(businessId, ct);
        return MapResult(result);
    }

    /// <summary>Updates an existing event.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEventDto request, CancellationToken ct)
    {
        var result = await eventService.UpdateAsync(id, request, ct);
        return MapResult(result);
    }

    /// <summary>Publishes an event, making it visible to end users.</summary>
    [HttpPatch("{id:int}/publish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Publish(int id, CancellationToken ct)
    {
        var result = await eventService.SetPublishedStatusAsync(id, true, ct);
        return MapResult(result);
    }

    /// <summary>Unpublishes an event, hiding it from end users.</summary>
    [HttpPatch("{id:int}/unpublish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unpublish(int id, CancellationToken ct)
    {
        var result = await eventService.SetPublishedStatusAsync(id, false, ct);
        return MapResult(result);
    }

    /// <summary>Deletes an event permanently.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await eventService.DeleteAsync(id, ct);
        return MapResult(result);
    }
}
