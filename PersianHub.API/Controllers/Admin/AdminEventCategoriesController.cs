using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin management of event categories.
/// </summary>
[Route("api/v1/admin/event-categories")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminEventCategoriesController(IEventCategoryService categoryService) : ApiControllerBase
{
    /// <summary>Returns all event categories including inactive ones.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EventCategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => MapResult(await categoryService.GetAllAsync(ct));

    /// <summary>Returns an event category by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EventCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await categoryService.GetByIdAsync(id, ct));

    /// <summary>Creates a new event category.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EventCategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] UpsertEventCategoryDto dto, CancellationToken ct)
    {
        var result = await categoryService.CreateAsync(dto, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Updates an existing event category.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EventCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpsertEventCategoryDto dto, CancellationToken ct)
        => MapResult(await categoryService.UpdateAsync(id, dto, ct));

    /// <summary>Activates an event category.</summary>
    [HttpPut("{id:int}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
        => MapResult(await categoryService.SetActiveStatusAsync(id, true, ct));

    /// <summary>Deactivates an event category.</summary>
    [HttpPut("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
        => MapResult(await categoryService.SetActiveStatusAsync(id, false, ct));

    /// <summary>Deletes an event category.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => MapResult(await categoryService.DeleteAsync(id, ct));
}
