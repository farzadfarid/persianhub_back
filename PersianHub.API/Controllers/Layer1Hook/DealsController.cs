using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Controllers.Layer1Hook;

/// <summary>
/// Public Deals browsing. Deals are promotions with coupon codes and discount values.
/// Public endpoints (no auth required): GetAll, GetActive, GetById, GetBySlug, GetByBusiness.
/// </summary>
[Route("api/v1/deals")]
public sealed class DealsController(IDealService dealService) : ApiControllerBase
{
    /// <summary>Creates a new deal for a business.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(DealDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateDealDto request, CancellationToken ct)
    {
        var result = await dealService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns all deals.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DealListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => MapResult(await dealService.GetAllAsync(ct));

    /// <summary>Returns a deal by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DealDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await dealService.GetByIdAsync(id, ct));

    /// <summary>Returns a deal by slug.</summary>
    [HttpGet("slug/{slug}")]
    [ProducesResponseType(typeof(DealDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug(string slug, CancellationToken ct)
        => MapResult(await dealService.GetBySlugAsync(slug, ct));

    /// <summary>Returns all published and currently valid deals.</summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IReadOnlyList<DealListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken ct)
        => MapResult(await dealService.GetActiveAsync(ct));

    /// <summary>Returns all deals for a specific business.</summary>
    [HttpGet("business/{businessId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<DealListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByBusiness(int businessId, CancellationToken ct)
        => MapResult(await dealService.GetByBusinessIdAsync(businessId, ct));

    /// <summary>Updates an existing deal.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(DealDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDealDto request, CancellationToken ct)
        => MapResult(await dealService.UpdateAsync(id, request, ct));

    /// <summary>Publishes a deal, making it visible to end users.</summary>
    [HttpPatch("{id:int}/publish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Publish(int id, CancellationToken ct)
        => MapResult(await dealService.SetPublishedStatusAsync(id, true, ct));

    /// <summary>Unpublishes a deal, hiding it from end users.</summary>
    [HttpPatch("{id:int}/unpublish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unpublish(int id, CancellationToken ct)
        => MapResult(await dealService.SetPublishedStatusAsync(id, false, ct));

    /// <summary>Deletes a deal permanently.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => MapResult(await dealService.DeleteAsync(id, ct));
}
