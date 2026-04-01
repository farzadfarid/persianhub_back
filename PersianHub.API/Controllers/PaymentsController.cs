using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Payment;
using PersianHub.API.Interfaces;

namespace PersianHub.API.Controllers;

/// <summary>
/// Payment lifecycle endpoints.
///
/// POST /api/v1/payments/create  — authenticated; initiates payment, returns gateway redirect URL.
/// GET  /api/v1/payments/callback — called by the payment gateway; verifies and activates subscription.
/// GET  /api/v1/payments/status/{subscriptionId} — authenticated; returns current payment status.
///
/// Security: the callback is unauthenticated (gateway cannot send JWT), but the business rule that
/// "only the gateway can activate a subscription" is enforced by verifying directly with the gateway
/// inside VerifyPaymentAsync — the frontend/client is never trusted.
/// </summary>
[Route("api/v1/payments")]
public sealed class PaymentsController(IPaymentService paymentService) : ApiControllerBase
{
    /// <summary>
    /// Initiates a subscription payment.
    /// Returns a <see cref="PaymentInitiatedDto"/> containing the gateway redirect URL.
    /// The subscription is created in Pending state until the gateway confirms payment.
    /// </summary>
    [HttpPost("create")]
    [Authorize]
    [ProducesResponseType(typeof(PaymentInitiatedDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreatePayment(
        [FromBody] CreatePaymentRequestDto request,
        CancellationToken ct = default)
    {
        var result = await paymentService.CreatePaymentRequestAsync(request, ct);
        return MapResult(result);
    }

    /// <summary>
    /// Payment gateway callback. Called by the gateway after the user completes (or cancels) payment.
    /// Verifies the transaction with the gateway and activates or fails the subscription.
    /// This endpoint is intentionally unauthenticated — gateways cannot send Bearer tokens.
    /// </summary>
    [HttpGet("callback")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaymentResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PaymentCallback(
        [FromQuery] string authority,
        [FromQuery] string status,
        CancellationToken ct = default)
    {
        var result = await paymentService.VerifyPaymentAsync(authority, status, ct);
        return MapResult(result);
    }

    /// <summary>
    /// Returns the current payment status for a subscription.
    /// Only accessible by the business owner or an Admin.
    /// </summary>
    [HttpGet("status/{subscriptionId:int}")]
    [Authorize]
    [ProducesResponseType(typeof(PaymentResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentStatus(
        int subscriptionId,
        CancellationToken ct = default)
    {
        var result = await paymentService.GetPaymentStatusAsync(subscriptionId, ct);
        return MapResult(result);
    }
}
