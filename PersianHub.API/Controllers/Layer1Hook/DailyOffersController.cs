using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Controllers.Layer1Hook;

/// <summary>
/// Manages daily promotional offers attached to businesses.
/// Supports full lifecycle: create, update, activate/deactivate, publish/unpublish.
/// </summary>
[Route("api/v1/daily-offers")]
public sealed class DailyOffersController(IDailyOfferService dailyOfferService) : ApiControllerBase
{
    /// <summary>Creates a new daily offer for a business.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(DailyOfferDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateDailyOfferDto request, CancellationToken ct)
    {
        var result = await dailyOfferService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns all daily offers.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DailyOfferListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await dailyOfferService.GetAllAsync(ct);
        return MapResult(result);
    }

    /// <summary>Returns a daily offer by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DailyOfferDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await dailyOfferService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns a daily offer by its slug.</summary>
    [HttpGet("slug/{slug}")]
    [ProducesResponseType(typeof(DailyOfferDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug(string slug, CancellationToken ct)
    {
        var result = await dailyOfferService.GetBySlugAsync(slug, ct);
        return MapResult(result);
    }

    /// <summary>Returns all active and published daily offers.</summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IReadOnlyList<DailyOfferListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken ct)
    {
        var result = await dailyOfferService.GetActiveAsync(ct);
        return MapResult(result);
    }

    /// <summary>Returns all daily offers for a specific business.</summary>
    [HttpGet("business/{businessId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<DailyOfferListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByBusiness(int businessId, CancellationToken ct)
    {
        var result = await dailyOfferService.GetByBusinessIdAsync(businessId, ct);
        return MapResult(result);
    }

    /// <summary>Updates an existing daily offer.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(DailyOfferDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDailyOfferDto request, CancellationToken ct)
    {
        var result = await dailyOfferService.UpdateAsync(id, request, ct);
        return MapResult(result);
    }

    /// <summary>Activates a daily offer.</summary>
    [HttpPatch("{id:int}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        var result = await dailyOfferService.SetActiveStatusAsync(id, true, ct);
        return MapResult(result);
    }

    /// <summary>Deactivates a daily offer.</summary>
    [HttpPatch("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
    {
        var result = await dailyOfferService.SetActiveStatusAsync(id, false, ct);
        return MapResult(result);
    }

    /// <summary>Publishes a daily offer, making it visible to end users.</summary>
    [HttpPatch("{id:int}/publish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Publish(int id, CancellationToken ct)
    {
        var result = await dailyOfferService.SetPublishedStatusAsync(id, true, ct);
        return MapResult(result);
    }

    /// <summary>Deletes a daily offer permanently.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await dailyOfferService.DeleteAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Unpublishes a daily offer, hiding it from end users.</summary>
    [HttpPatch("{id:int}/unpublish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unpublish(int id, CancellationToken ct)
    {
        var result = await dailyOfferService.SetPublishedStatusAsync(id, false, ct);
        return MapResult(result);
    }
}
