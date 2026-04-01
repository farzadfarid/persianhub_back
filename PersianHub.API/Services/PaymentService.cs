using Microsoft.EntityFrameworkCore;
using PersianHub.API.Auth;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Payment;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Interfaces;

namespace PersianHub.API.Services;

/// <summary>
/// ZarinPal-style payment gateway implementation.
///
/// Integration points are marked with "// GATEWAY:" comments.
/// Replace stub logic with real HTTP calls to the gateway SDK when ready.
///
/// Activation contract:
///   - subscription.Status is NEVER set to Active before gateway verification.
///   - Only VerifyPaymentAsync may activate a subscription.
///   - A failed payment sets Status=Cancelled, PaymentStatus=Failed so the business can retry.
/// </summary>
public sealed class PaymentService(
    ApplicationDbContext db,
    ICurrentUserService currentUser,
    IDateTimeProvider clock,
    IAuditLogService audit) : IPaymentService
{
    public async Task<Result<PaymentInitiatedDto>> CreatePaymentRequestAsync(
        CreatePaymentRequestDto request, CancellationToken ct = default)
    {
        // --- Validate business ownership ---
        var business = await db.Businesses.FirstOrDefaultAsync(b => b.Id == request.BusinessId, ct);
        if (business is null)
            return Result<PaymentInitiatedDto>.Failure(
                $"Business with id {request.BusinessId} not found.", ErrorCodes.NotFound);

        var role = currentUser.GetRole();
        var userId = currentUser.GetUserId();
        if (role != AppRoles.Admin && (userId == 0 || business.OwnerUserId != userId))
            return Result<PaymentInitiatedDto>.Failure(
                "You do not have permission to manage subscriptions for this business.", ErrorCodes.Forbidden);

        // --- Validate plan ---
        var plan = await db.SubscriptionPlans.FirstOrDefaultAsync(p => p.Id == request.SubscriptionPlanId, ct);
        if (plan is null)
            return Result<PaymentInitiatedDto>.Failure(
                $"Subscription plan with id {request.SubscriptionPlanId} not found.", ErrorCodes.NotFound);

        if (!plan.IsActive)
            return Result<PaymentInitiatedDto>.Failure(
                $"Subscription plan '{plan.Code}' is no longer active.", ErrorCodes.Conflict);

        if (plan.Price == 0)
            return Result<PaymentInitiatedDto>.Failure(
                "Free plans do not require payment. Use the subscriptions endpoint directly.", ErrorCodes.Conflict);

        // --- Enforce single active/pending subscription ---
        var hasActiveOrPending = await db.Subscriptions.AnyAsync(
            s => s.BusinessId == request.BusinessId &&
                 (s.Status == SubscriptionStatus.Active || s.Status == SubscriptionStatus.Pending), ct);

        if (hasActiveOrPending)
            return Result<PaymentInitiatedDto>.Failure(
                "This business already has an active or pending subscription. Cancel it before creating a new one.",
                ErrorCodes.Conflict);

        // --- Create subscription in Pending state ---
        var now = clock.UtcNow;
        var endDate = plan.BillingCycle == SubscriptionBillingCycle.Yearly
            ? now.AddYears(1)
            : now.AddMonths(1);

        var subscription = new Subscription
        {
            BusinessId = request.BusinessId,
            SubscriptionPlanId = request.SubscriptionPlanId,
            StartDateUtc = now,
            EndDateUtc = endDate,
            Status = SubscriptionStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            AutoRenew = request.AutoRenew,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.Subscriptions.Add(subscription);
        await db.SaveChangesAsync(ct);

        // --- GATEWAY: Create payment request ---
        // Replace this stub with the real gateway SDK call:
        //
        // var gatewayResult = await _zarinpalClient.PaymentRequest(new ZarinpalRequest
        // {
        //     MerchantId  = _config["ZarinPal:MerchantId"],
        //     Amount      = (int)(plan.Price * 100),   // convert SEK to öre, or use gateway's currency
        //     Description = $"PersianHub subscription — {plan.Name}",
        //     CallbackUrl = request.CallbackUrl,
        //     Metadata    = new { subscriptionId = subscription.Id }
        // });
        //
        // if (!gatewayResult.IsSuccess)
        // {
        //     db.Subscriptions.Remove(subscription);
        //     await db.SaveChangesAsync(ct);
        //     return Result<PaymentInitiatedDto>.Failure("Payment gateway error: " + gatewayResult.Message, ErrorCodes.ExternalServiceError);
        // }
        //
        // var authority = gatewayResult.Authority;
        // var paymentUrl = $"https://www.zarinpal.com/pg/StartPay/{authority}";

        // STUB: generate a deterministic authority for testing
        var authority = $"STUB-{subscription.Id}-{Guid.NewGuid():N}";
        var paymentUrl = $"{request.CallbackUrl}?authority={authority}&status=OK&stub=true";

        // Store the gateway authority as ExternalReference so VerifyPaymentAsync can look it up.
        subscription.ExternalReference = authority;
        subscription.UpdatedAtUtc = clock.UtcNow;
        await db.SaveChangesAsync(ct);

        await audit.WriteAsync(AuditActions.PaymentRequested, "Subscription", subscription.Id.ToString(),
            new { subscription.BusinessId, planId = request.SubscriptionPlanId }, ct);

        return Result<PaymentInitiatedDto>.Success(new PaymentInitiatedDto(subscription.Id, authority, paymentUrl));
    }

    public async Task<Result<PaymentResultDto>> VerifyPaymentAsync(
        string authority, string status, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(authority))
            return Result<PaymentResultDto>.Failure("Authority is required.", ErrorCodes.ValidationFailed);

        // --- Find the subscription that was created for this authority ---
        var subscription = await db.Subscriptions
            .Include(s => s.SubscriptionPlan)
            .FirstOrDefaultAsync(s => s.ExternalReference == authority, ct);

        if (subscription is null)
            return Result<PaymentResultDto>.Failure(
                "No subscription found for the provided authority.", ErrorCodes.NotFound);

        if (subscription.Status != SubscriptionStatus.Pending)
            return Result<PaymentResultDto>.Failure(
                "This subscription is no longer in a pending state.", ErrorCodes.Conflict);

        var now = clock.UtcNow;

        // --- GATEWAY: Verify payment with the gateway ---
        // Replace this stub with the real gateway verification call:
        //
        // var verifyResult = await _zarinpalClient.PaymentVerification(new ZarinpalVerifyRequest
        // {
        //     MerchantId = _config["ZarinPal:MerchantId"],
        //     Amount     = (int)(subscription.SubscriptionPlan.Price * 100),
        //     Authority  = authority
        // });
        //
        // bool isVerified = verifyResult.Status == 100 || verifyResult.Status == 101;
        // string? refId   = isVerified ? verifyResult.RefId.ToString() : null;

        // STUB: treat status=="OK" as successful verification
        var isVerified = string.Equals(status, "OK", StringComparison.OrdinalIgnoreCase);
        var refId = isVerified ? $"REF-{Guid.NewGuid():N}" : null;

        if (isVerified)
        {
            subscription.Status = SubscriptionStatus.Active;
            subscription.PaymentStatus = PaymentStatus.Paid;
            subscription.PaymentReference = refId;
            subscription.ActivatedAtUtc = now;
            subscription.UpdatedAtUtc = now;

            await db.SaveChangesAsync(ct);

            await audit.WriteAsync(AuditActions.PaymentVerified, "Subscription", subscription.Id.ToString(),
                new { subscription.BusinessId }, ct);
            await audit.WriteAsync(AuditActions.SubscriptionActivated, "Subscription", subscription.Id.ToString(),
                new { subscription.BusinessId }, ct);

            return Result<PaymentResultDto>.Success(new PaymentResultDto(
                subscription.Id, true, PaymentStatus.Paid, refId,
                "Payment verified. Subscription is now active."));
        }
        else
        {
            // Cancel the subscription so the business can initiate a fresh payment attempt.
            subscription.Status = SubscriptionStatus.Cancelled;
            subscription.PaymentStatus = PaymentStatus.Failed;
            subscription.UpdatedAtUtc = now;

            await db.SaveChangesAsync(ct);

            await audit.WriteAsync(AuditActions.PaymentFailed, "Subscription", subscription.Id.ToString(),
                new { subscription.BusinessId }, ct);

            return Result<PaymentResultDto>.Success(new PaymentResultDto(
                subscription.Id, false, PaymentStatus.Failed, null,
                "Payment was not completed. The subscription has been cancelled. Please try again."));
        }
    }

    public async Task<Result<PaymentResultDto>> GetPaymentStatusAsync(
        int subscriptionId, CancellationToken ct = default)
    {
        var subscription = await db.Subscriptions
            .AsNoTracking()
            .Include(s => s.Business)
            .FirstOrDefaultAsync(s => s.Id == subscriptionId, ct);

        if (subscription is null)
            return Result<PaymentResultDto>.Failure(
                $"Subscription with id {subscriptionId} not found.", ErrorCodes.NotFound);

        var role = currentUser.GetRole();
        var userId = currentUser.GetUserId();
        if (role != AppRoles.Admin && (userId == 0 || subscription.Business.OwnerUserId != userId))
            return Result<PaymentResultDto>.Failure(
                "You do not have permission to view this subscription.", ErrorCodes.Forbidden);

        var message = subscription.PaymentStatus switch
        {
            PaymentStatus.Paid    => "Payment successful. Subscription is active.",
            PaymentStatus.Failed  => "Payment failed. Please initiate a new subscription.",
            PaymentStatus.Pending => "Payment is pending. Complete the payment to activate your subscription.",
            _                     => "Unknown payment status."
        };

        return Result<PaymentResultDto>.Success(new PaymentResultDto(
            subscription.Id, subscription.PaymentStatus == PaymentStatus.Paid,
            subscription.PaymentStatus, subscription.PaymentReference, message));
    }
}
