using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Admin;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin management of subscription plans (monetization control).
/// Plans are not hard-deleted to preserve subscription history integrity.
/// </summary>
[Route("api/v1/admin/subscription-plans")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminSubscriptionPlansController(
    ISubscriptionPlanService planService,
    IAdminSubscriptionPlanService adminPlanService) : ApiControllerBase
{
    /// <summary>Returns all subscription plans (including inactive).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SubscriptionPlanListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => MapResult(await planService.GetAllAsync(ct));

    /// <summary>Returns full details for a subscription plan.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SubscriptionPlanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await planService.GetByIdAsync(id, ct));

    /// <summary>Creates a new subscription plan.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(SubscriptionPlanDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateSubscriptionPlanAdminDto dto, CancellationToken ct)
    {
        var result = await adminPlanService.CreateAsync(dto, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Updates an existing subscription plan. Plan code is immutable after creation.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(SubscriptionPlanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSubscriptionPlanAdminDto dto, CancellationToken ct)
        => MapResult(await adminPlanService.UpdateAsync(id, dto, ct));

    /// <summary>Activates a plan — makes it available for new subscriptions.</summary>
    [HttpPut("{id:int}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
        => MapResult(await adminPlanService.SetActiveStatusAsync(id, true, ct));

    /// <summary>Deactivates a plan — prevents new subscriptions; existing subscriptions unaffected.</summary>
    [HttpPut("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
        => MapResult(await adminPlanService.SetActiveStatusAsync(id, false, ct));
}
