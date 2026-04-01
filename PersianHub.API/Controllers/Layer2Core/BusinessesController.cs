using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Layer2Core;

/// <summary>
/// Manages business profiles in the directory.
/// Businesses are the paying actors on PersianHub — they subscribe to plans for visibility.
/// </summary>
[Route("api/v1/businesses")]
public sealed class BusinessesController(IBusinessService businessService) : ApiControllerBase
{
    /// <summary>Returns all businesses ordered by featured status then name. Public.</summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<BusinessListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await businessService.GetAllAsync(ct);
        return MapResult(result);
    }

    /// <summary>Returns the authenticated user's own businesses.</summary>
    [HttpGet("my")]
    [Authorize]
    [ProducesResponseType(typeof(IReadOnlyList<BusinessListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMy(CancellationToken ct)
    {
        var result = await businessService.GetMyBusinessesAsync(ct);
        return MapResult(result);
    }

    /// <summary>Returns a business by its internal id. Public.</summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BusinessDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await businessService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns a business by its URL slug. Public.</summary>
    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BusinessDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug(string slug, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return BadRequest(Problem(detail: "Slug is required.", title: "Bad Request", statusCode: 400));

        var result = await businessService.GetBySlugAsync(slug, ct);
        return MapResult(result);
    }

    /// <summary>
    /// Creates a new business profile. OwnerUserId is set automatically from the authenticated user.
    /// If the user's role is User it is promoted to BusinessOwner automatically.
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(BusinessDetailsDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBusinessRequestDto request, CancellationToken ct)
    {
        var result = await businessService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>
    /// Updates an existing business profile.
    /// Only the owner or an Admin may update a business.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(BusinessDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBusinessRequestDto request, CancellationToken ct)
    {
        var result = await businessService.UpdateAsync(id, request, ct);
        return MapResult(result);
    }

    /// <summary>
    /// Sets the active/inactive status of a business.
    /// Only the owner or an Admin may change status.
    /// Inactive businesses are hidden from public listings.
    /// </summary>
    [HttpPatch("{id:int}/status")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetStatus(int id, [FromBody] SetActiveStatusDto request, CancellationToken ct)
    {
        var result = await businessService.SetActiveStatusAsync(id, request.IsActive, ct);
        return MapResult(result);
    }
}
