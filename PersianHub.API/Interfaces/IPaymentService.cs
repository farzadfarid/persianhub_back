using PersianHub.API.Common;
using PersianHub.API.DTOs.Payment;

namespace PersianHub.API.Interfaces;

/// <summary>
/// Abstracts the payment gateway so implementations can be swapped (ZarinPal, Stripe, etc.)
/// without changing the controller or any other service.
///
/// Flow:
///   1. CreatePaymentRequestAsync — validates ownership + plan, creates Pending subscription,
///      calls gateway, stores authority as ExternalReference, returns payment URL.
///   2. VerifyPaymentAsync       — called from the gateway callback; verifies transaction with
///      the gateway and activates or fails the subscription accordingly.
///   3. GetPaymentStatusAsync    — lightweight status check by subscription id.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Initiates a payment for a subscription plan.
    /// Returns a <see cref="PaymentInitiatedDto"/> containing the gateway redirect URL.
    /// </summary>
    Task<Result<PaymentInitiatedDto>> CreatePaymentRequestAsync(CreatePaymentRequestDto request, CancellationToken ct = default);

    /// <summary>
    /// Verifies a gateway callback and activates or fails the associated subscription.
    /// </summary>
    /// <param name="authority">The gateway-issued session token from the callback query string.</param>
    /// <param name="status">The status string from the callback (e.g. "OK" or "NOK").</param>
    Task<Result<PaymentResultDto>> VerifyPaymentAsync(string authority, string status, CancellationToken ct = default);

    /// <summary>
    /// Returns the current payment status for a subscription.
    /// </summary>
    Task<Result<PaymentResultDto>> GetPaymentStatusAsync(int subscriptionId, CancellationToken ct = default);
}
