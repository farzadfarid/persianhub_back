using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Layer2Core;

/// <summary>
/// Read-only access to subscription plans. All endpoints are public (pricing page).
/// Plan management (create/update/delete) is reserved for the admin panel (future task).
/// </summary>
[Route("api/v1/subscription-plans")]
[AllowAnonymous]
public sealed class SubscriptionPlansController(ISubscriptionPlanService planService) : ApiControllerBase
{
    /// <summary>Returns all subscription plans regardless of status.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SubscriptionPlanListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await planService.GetAllAsync(ct);
        return MapResult(result);
    }

    /// <summary>Returns only active subscription plans — suitable for the public pricing page.</summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IReadOnlyList<SubscriptionPlanListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken ct)
    {
        var result = await planService.GetActiveAsync(ct);
        return MapResult(result);
    }

    /// <summary>Returns a subscription plan by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SubscriptionPlanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await planService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns a subscription plan by its code (e.g. "FREE", "BASIC", "PREMIUM"). Case-insensitive.</summary>
    [HttpGet("code/{code}")]
    [ProducesResponseType(typeof(SubscriptionPlanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCode(string code, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest(Problem(detail: "Code is required.", title: "Bad Request", statusCode: 400));

        var result = await planService.GetByCodeAsync(code, ct);
        return MapResult(result);
    }
}
