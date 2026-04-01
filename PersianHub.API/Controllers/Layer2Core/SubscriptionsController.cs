using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Layer2Core;

/// <summary>
/// Manages business subscriptions — the primary monetization mechanism in PersianHub.
/// A business may only have one Active subscription at a time; cancel before creating a new one.
/// All endpoints require authentication. Ownership is enforced in the service layer.
/// </summary>
[Route("api/v1")]
[Authorize]
public sealed class SubscriptionsController(ISubscriptionService subscriptionService) : ApiControllerBase
{
    /// <summary>
    /// Creates a new subscription for a business owned by the current user.
    /// Start and end dates are computed server-side from the plan's billing cycle.
    /// </summary>
    [HttpPost("subscriptions")]
    [ProducesResponseType(typeof(SubscriptionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateSubscriptionRequestDto request, CancellationToken ct)
    {
        var result = await subscriptionService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns a subscription by its id. Only the business owner or Admin may access it.</summary>
    [HttpGet("subscriptions/{id:int}")]
    [ProducesResponseType(typeof(SubscriptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await subscriptionService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>
    /// Cancels a subscription. Only the business owner or Admin may cancel.
    /// Subscription records are never deleted — status is set to Cancelled.
    /// </summary>
    [HttpPost("subscriptions/{id:int}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        var result = await subscriptionService.CancelAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns full subscription history for a business. Owner or Admin only.</summary>
    [HttpGet("businesses/{businessId:int}/subscriptions")]
    [ProducesResponseType(typeof(IReadOnlyList<SubscriptionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetForBusiness(int businessId, CancellationToken ct)
    {
        var result = await subscriptionService.GetForBusinessAsync(businessId, ct);
        return MapResult(result);
    }

    /// <summary>
    /// Returns the currently active subscription for a business. Owner or Admin only.
    /// Returns 404 when no active subscription exists — treat as "no subscription", not an error.
    /// </summary>
    [HttpGet("businesses/{businessId:int}/subscriptions/active")]
    [ProducesResponseType(typeof(SubscriptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActiveForBusiness(int businessId, CancellationToken ct)
    {
        var result = await subscriptionService.GetActiveForBusinessAsync(businessId, ct);

        if (!result.IsSuccess)
            return MapResult(result);

        if (result.Value is null)
            return NotFound(Problem(detail: "No active subscription found for this business.", title: "Not Found", statusCode: 404));

        return Ok(result.Value);
    }
}
