using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin management of Deals. All operations are admin-only and independent of owner endpoints.
/// Toggle-active operates on IsPublished. Delete sets IsPublished = false (soft-delete pattern).
/// </summary>
[Route("api/v1/admin/deals")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminDealsController(IAdminDealService dealService) : ApiControllerBase
{
    /// <summary>Paginated list of all deals with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? businessId,
        [FromQuery] bool? isPublished,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await dealService.GetAllAsync(businessId, isPublished, search, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for a deal by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminDealDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await dealService.GetByIdAsync(id, ct));

    /// <summary>Creates a new deal.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(AdminDealDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AdminCreateDealDto dto, CancellationToken ct)
    {
        var result = await dealService.CreateAsync(dto, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Updates an existing deal.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(AdminDealDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] AdminUpdateDealDto dto, CancellationToken ct)
        => MapResult(await dealService.UpdateAsync(id, dto, ct));

    /// <summary>Toggles IsPublished on a deal.</summary>
    [HttpPatch("{id:int}/toggle-active")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleActive(int id, CancellationToken ct)
        => MapResult(await dealService.TogglePublishedAsync(id, ct));

    /// <summary>Soft-deletes a deal by unpublishing it.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => MapResult(await dealService.DeleteAsync(id, ct));
}
