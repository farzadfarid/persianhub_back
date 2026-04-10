using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin management of business tags.
/// </summary>
[Route("api/v1/admin/business-tags")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminBusinessTagsController(IBusinessTagService tagService) : ApiControllerBase
{
    /// <summary>Returns all business tags including inactive ones.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BusinessTagDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => MapResult(await tagService.GetAllAsync(ct));

    /// <summary>Returns a business tag by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BusinessTagDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await tagService.GetByIdAsync(id, ct));

    /// <summary>Creates a new business tag.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BusinessTagDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] UpsertBusinessTagDto dto, CancellationToken ct)
    {
        var result = await tagService.CreateAsync(dto, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Updates an existing business tag.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BusinessTagDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpsertBusinessTagDto dto, CancellationToken ct)
        => MapResult(await tagService.UpdateAsync(id, dto, ct));

    /// <summary>Activates a business tag.</summary>
    [HttpPut("{id:int}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
        => MapResult(await tagService.SetActiveStatusAsync(id, true, ct));

    /// <summary>Deactivates a business tag.</summary>
    [HttpPut("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
        => MapResult(await tagService.SetActiveStatusAsync(id, false, ct));

    /// <summary>Deletes a business tag. Fails if businesses are linked.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => MapResult(await tagService.DeleteAsync(id, ct));
}
