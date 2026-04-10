using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin management of referral codes.
/// Admin can create codes for any user and toggle their active state.
/// Code values are immutable after creation to preserve active referral links.
/// </summary>
[Route("api/v1/admin/referral-codes")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminReferralCodesController(IAdminReferralCodeService codeService) : ApiControllerBase
{
    /// <summary>Paginated list of referral codes with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? userId,
        [FromQuery] bool? isActive,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await codeService.GetAllAsync(userId, isActive, search, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for a referral code by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminReferralCodeDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await codeService.GetByIdAsync(id, ct));

    /// <summary>Creates a referral code and assigns it to a user.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(AdminReferralCodeDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] AdminCreateReferralCodeDto dto, CancellationToken ct)
        => MapCreated(await codeService.CreateAsync(dto, ct), nameof(GetById), null);

    /// <summary>Toggles the IsActive flag of a referral code.</summary>
    [HttpPatch("{id:int}/toggle-active")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleActive(int id, CancellationToken ct)
        => MapResult(await codeService.ToggleActiveAsync(id, ct));
}
