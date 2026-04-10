using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin payment visibility backed by the Subscription table.
/// No standalone Payment entity exists in the current schema.
/// Payment data (status, reference, gateway authority) is stored on Subscription.
/// This controller is read-only — payments are initiated by the payment gateway flow.
/// </summary>
[Route("api/v1/admin/payments")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminPaymentsController(IAdminPaymentService paymentService) : ApiControllerBase
{
    /// <summary>Paginated list of subscription payment records with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? businessId,
        [FromQuery] PaymentStatus? paymentStatus,
        [FromQuery] DateTime? fromUtc,
        [FromQuery] DateTime? toUtc,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await paymentService.GetAllAsync(
            businessId, paymentStatus, fromUtc, toUtc, search, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full payment details for a subscription by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminPaymentDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await paymentService.GetByIdAsync(id, ct));
}
