using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Layer2Core;

/// <summary>
/// Tracks user-to-business interaction events (ViewBusiness, ClickPhone, ClickWebsite, etc.)
/// Used for monetization analytics and future ranking/recommendation features.
/// </summary>
[Route("api/v1/interactions")]
public sealed class InteractionsController(IInteractionService interactionService) : ApiControllerBase
{
    /// <summary>Records a new interaction event.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(InteractionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateInteractionDto request, CancellationToken ct)
    {
        var result = await interactionService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns an interaction by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InteractionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await interactionService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns all interactions for a specific business.</summary>
    [HttpGet("business/{businessId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<InteractionListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByBusiness(int businessId, CancellationToken ct)
    {
        var result = await interactionService.GetByBusinessIdAsync(businessId, ct);
        return MapResult(result);
    }

    /// <summary>Returns all interactions recorded for a specific user.</summary>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<InteractionListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUser(int userId, CancellationToken ct)
    {
        var result = await interactionService.GetByUserIdAsync(userId, ct);
        return MapResult(result);
    }

    /// <summary>Returns aggregated interaction counts (views, clicks, contact events) for a business.</summary>
    [HttpGet("business/{businessId:int}/counts")]
    [ProducesResponseType(typeof(InteractionCountsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCountsByBusiness(int businessId, CancellationToken ct)
    {
        var result = await interactionService.GetCountsByBusinessIdAsync(businessId, ct);
        return MapResult(result);
    }
}
