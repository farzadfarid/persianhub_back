using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin management of Daily Offers. All operations are admin-only.
/// Does NOT reuse owner endpoints — queries all offers across all businesses.
/// </summary>
[Route("api/v1/admin/daily-offers")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminDailyOffersController(IAdminDailyOfferService offerService) : ApiControllerBase
{
    /// <summary>Paginated list of all daily offers with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? businessId,
        [FromQuery] bool? isActive,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await offerService.GetAllAsync(businessId, isActive, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for a daily offer by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminDailyOfferDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await offerService.GetByIdAsync(id, ct));

    /// <summary>Creates a new daily offer for any business.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(AdminDailyOfferDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AdminCreateDailyOfferDto dto, CancellationToken ct)
    {
        var result = await offerService.CreateAsync(dto, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Updates an existing daily offer.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(AdminDailyOfferDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] AdminUpdateDailyOfferDto dto, CancellationToken ct)
        => MapResult(await offerService.UpdateAsync(id, dto, ct));

    /// <summary>Toggles IsActive on a daily offer.</summary>
    [HttpPatch("{id:int}/toggle-active")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleActive(int id, CancellationToken ct)
        => MapResult(await offerService.ToggleActiveAsync(id, ct));

    /// <summary>Soft-deletes by setting IsActive and IsPublished to false.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => MapResult(await offerService.DeleteAsync(id, ct));
}
