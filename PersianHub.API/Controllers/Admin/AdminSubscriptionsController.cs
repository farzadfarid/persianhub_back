using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin oversight of all subscriptions across the platform.
/// Subscription records are never deleted — cancellation is the only status change exposed here.
/// </summary>
[Route("api/v1/admin/subscriptions")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminSubscriptionsController(ISubscriptionService subscriptionService) : ApiControllerBase
{
    /// <summary>Returns all subscriptions across all businesses.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SubscriptionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => MapResult(await subscriptionService.GetAllAsync(ct));

    /// <summary>Returns a subscription by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SubscriptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await subscriptionService.GetByIdAsync(id, ct));

    /// <summary>Force-cancels a subscription. Status history is preserved.</summary>
    [HttpPost("{id:int}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        => MapResult(await subscriptionService.CancelAsync(id, ct));
}
