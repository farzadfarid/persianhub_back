using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin management of all businesses on the platform.
/// Admin bypasses ownership restrictions (enforced in BusinessService).
/// Deletion is not exposed — use deactivate to preserve data integrity.
/// </summary>
[Route("api/v1/admin/businesses")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminBusinessesController(
    IBusinessService businessService,
    ApplicationDbContext db) : ApiControllerBase
{
    /// <summary>Returns all businesses with owner info.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AdminBusinessListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var businesses = await db.Businesses
            .AsNoTracking()
            .OrderByDescending(b => b.Id)
            .Select(b => new AdminBusinessListItemDto(
                b.Id,
                b.Name,
                b.NameFa,
                b.Slug,
                b.City,
                b.PhoneNumber,
                b.Email,
                b.LogoUrl,
                b.IsVerified,
                b.IsFeatured,
                b.IsActive,
                b.OwnerUserId,
                b.OwnerUser != null ? b.OwnerUser.FirstName : null,
                b.OwnerUser != null ? b.OwnerUser.LastName : null,
                b.OwnerUser != null ? b.OwnerUser.Email : null,
                b.CreatedAtUtc
            ))
            .ToListAsync(ct);

        return Ok(businesses);
    }

    /// <summary>Returns full details for any business by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BusinessDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await businessService.GetByIdAsync(id, ct));

    /// <summary>Activates a business — makes it visible in public listings.</summary>
    [HttpPut("{id:int}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
        => MapResult(await businessService.SetActiveStatusAsync(id, true, ct));

    /// <summary>Deactivates a business — hides it from public listings. Does not delete data.</summary>
    [HttpPut("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
        => MapResult(await businessService.SetActiveStatusAsync(id, false, ct));
}
