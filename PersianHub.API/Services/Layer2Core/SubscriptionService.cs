using Microsoft.EntityFrameworkCore;
using PersianHub.API.Auth;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Interfaces;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Services.Layer2Core;

/// <summary>
/// Manages business subscriptions — the primary monetization flow.
///
/// Payment lifecycle:
/// - Free plans (Price == 0) are created directly as Active with PaymentStatus = Paid.
/// - Paid plans are created as Pending/PaymentStatus=Pending via IPaymentService.
///   PaymentService.VerifyPaymentAsync activates them after gateway confirmation.
/// - ExternalReference holds the gateway authority/session token.
/// - PaymentReference holds the final transaction id returned on successful verification.
/// </summary>
public sealed class SubscriptionService(
    ApplicationDbContext db,
    IDateTimeProvider clock,
    ICurrentUserService currentUser,
    IAuditLogService audit) : ISubscriptionService
{
    /// <summary>
    /// Creates a subscription for a business owned by the current authenticated user.
    /// Business rule: only one Active or Pending subscription per business at a time.
    /// Free plans (Price == 0) are activated immediately.
    /// Paid plans must go through IPaymentService — this method returns Pending.
    /// StartDateUtc and EndDateUtc are computed server-side from the plan's BillingCycle.
    /// </summary>
    public async Task<Result<SubscriptionDto>> CreateAsync(CreateSubscriptionRequestDto request, CancellationToken ct = default)
    {
        var business = await db.Businesses.FirstOrDefaultAsync(b => b.Id == request.BusinessId, ct);
        if (business is null)
            return Result<SubscriptionDto>.Failure($"Business with id {request.BusinessId} not found.", ErrorCodes.NotFound);

        var accessCheck = CheckBusinessAccess(business);
        if (!accessCheck.IsSuccess)
            return Result<SubscriptionDto>.Failure(accessCheck.Error!, accessCheck.ErrorCode);

        var plan = await db.SubscriptionPlans.FirstOrDefaultAsync(p => p.Id == request.SubscriptionPlanId, ct);
        if (plan is null)
            return Result<SubscriptionDto>.Failure($"Subscription plan with id {request.SubscriptionPlanId} not found.", ErrorCodes.NotFound);

        if (!plan.IsActive)
            return Result<SubscriptionDto>.Failure($"Subscription plan '{plan.Code}' is no longer active.", ErrorCodes.Conflict);

        // Enforce single-active/pending subscription rule.
        var hasActiveOrPending = await db.Subscriptions.AnyAsync(
            s => s.BusinessId == request.BusinessId &&
                 (s.Status == SubscriptionStatus.Active || s.Status == SubscriptionStatus.Pending), ct);

        if (hasActiveOrPending)
            return Result<SubscriptionDto>.Failure(
                "This business already has an active or pending subscription. Cancel it before creating a new one.",
                ErrorCodes.Conflict);

        var now = clock.UtcNow;
        var endDate = plan.BillingCycle == SubscriptionBillingCycle.Yearly
            ? now.AddYears(1)
            : now.AddMonths(1);

        // Free plans are activated immediately; paid plans require payment confirmation via IPaymentService.
        var isFree = plan.Price == 0;
        var subscription = new Subscription
        {
            BusinessId = request.BusinessId,
            SubscriptionPlanId = request.SubscriptionPlanId,
            StartDateUtc = now,
            EndDateUtc = endDate,
            Status = isFree ? SubscriptionStatus.Active : SubscriptionStatus.Pending,
            PaymentStatus = isFree ? PaymentStatus.Paid : PaymentStatus.Pending,
            ActivatedAtUtc = isFree ? now : null,
            AutoRenew = request.AutoRenew,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.Subscriptions.Add(subscription);
        await db.SaveChangesAsync(ct);

        await audit.WriteAsync(AuditActions.SubscriptionCreated, "Subscription", subscription.Id.ToString(),
            new { subscription.BusinessId, planCode = plan.Code, isFree, subscription.Status }, ct);

        return Result<SubscriptionDto>.Success(ToDto(subscription, plan.Name, plan.Code));
    }

    public async Task<Result<SubscriptionDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var sub = await db.Subscriptions
            .AsNoTracking()
            .Include(s => s.SubscriptionPlan)
            .Include(s => s.Business)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (sub is null)
            return Result<SubscriptionDto>.Failure($"Subscription with id {id} not found.", ErrorCodes.NotFound);

        var accessCheck = CheckBusinessAccess(sub.Business);
        if (!accessCheck.IsSuccess)
            return Result<SubscriptionDto>.Failure(accessCheck.Error!, accessCheck.ErrorCode);

        return Result<SubscriptionDto>.Success(ToDto(sub, sub.SubscriptionPlan.Name, sub.SubscriptionPlan.Code));
    }

    public async Task<Result<IReadOnlyList<SubscriptionDto>>> GetForBusinessAsync(int businessId, CancellationToken ct = default)
    {
        var business = await db.Businesses.AsNoTracking().FirstOrDefaultAsync(b => b.Id == businessId, ct);
        if (business is null)
            return Result<IReadOnlyList<SubscriptionDto>>.Failure($"Business with id {businessId} not found.", ErrorCodes.NotFound);

        var accessCheck = CheckBusinessAccess(business);
        if (!accessCheck.IsSuccess)
            return Result<IReadOnlyList<SubscriptionDto>>.Failure(accessCheck.Error!, accessCheck.ErrorCode);

        var subs = await db.Subscriptions
            .AsNoTracking()
            .Include(s => s.SubscriptionPlan)
            .Where(s => s.BusinessId == businessId)
            .OrderByDescending(s => s.CreatedAtUtc)
            .ToListAsync(ct);

        var dtos = subs.Select(s => ToDto(s, s.SubscriptionPlan.Name, s.SubscriptionPlan.Code)).ToList();
        return Result<IReadOnlyList<SubscriptionDto>>.Success(dtos);
    }

    public async Task<Result<SubscriptionDto?>> GetActiveForBusinessAsync(int businessId, CancellationToken ct = default)
    {
        var business = await db.Businesses.AsNoTracking().FirstOrDefaultAsync(b => b.Id == businessId, ct);
        if (business is null)
            return Result<SubscriptionDto?>.Failure($"Business with id {businessId} not found.", ErrorCodes.NotFound);

        var accessCheck = CheckBusinessAccess(business);
        if (!accessCheck.IsSuccess)
            return Result<SubscriptionDto?>.Failure(accessCheck.Error!, accessCheck.ErrorCode);

        var sub = await db.Subscriptions
            .AsNoTracking()
            .Include(s => s.SubscriptionPlan)
            .FirstOrDefaultAsync(s => s.BusinessId == businessId && s.Status == SubscriptionStatus.Active, ct);

        var dto = sub is null ? null : ToDto(sub, sub.SubscriptionPlan.Name, sub.SubscriptionPlan.Code);
        return Result<SubscriptionDto?>.Success(dto);
    }

    public async Task<Result<IReadOnlyList<SubscriptionDto>>> GetAllAsync(CancellationToken ct = default)
    {
        if (currentUser.GetRole() != AppRoles.Admin)
            return Result<IReadOnlyList<SubscriptionDto>>.Failure("Admin access required.", ErrorCodes.Forbidden);

        var subs = await db.Subscriptions
            .AsNoTracking()
            .Include(s => s.SubscriptionPlan)
            .OrderByDescending(s => s.CreatedAtUtc)
            .ToListAsync(ct);

        var dtos = subs.Select(s => ToDto(s, s.SubscriptionPlan.Name, s.SubscriptionPlan.Code)).ToList();
        return Result<IReadOnlyList<SubscriptionDto>>.Success(dtos);
    }

    public async Task<Result> CancelAsync(int subscriptionId, CancellationToken ct = default)
    {
        var sub = await db.Subscriptions
            .Include(s => s.Business)
            .FirstOrDefaultAsync(s => s.Id == subscriptionId, ct);

        if (sub is null)
            return Result.Failure($"Subscription with id {subscriptionId} not found.", ErrorCodes.NotFound);

        var accessCheck = CheckBusinessAccess(sub.Business);
        if (!accessCheck.IsSuccess)
            return accessCheck;

        if (sub.Status == SubscriptionStatus.Cancelled)
            return Result.Failure("Subscription is already cancelled.", ErrorCodes.Conflict);

        if (sub.Status == SubscriptionStatus.Expired)
            return Result.Failure("Cannot cancel an already expired subscription.", ErrorCodes.Conflict);

        sub.Status = SubscriptionStatus.Cancelled;
        sub.UpdatedAtUtc = clock.UtcNow;
        await db.SaveChangesAsync(ct);

        await audit.WriteAsync(AuditActions.SubscriptionCancelled, "Subscription", sub.Id.ToString(),
            new { sub.BusinessId }, ct);

        if (currentUser.GetRole() == AppRoles.Admin)
            await audit.WriteAsync(AuditActions.AdminSubscriptionForceCancelled, "Subscription", sub.Id.ToString(),
                new { sub.BusinessId }, ct);

        return Result.Success();
    }

    /// <summary>
    /// Returns Success if the current user is Admin or owns the given business.
    /// Admin bypasses all ownership restrictions.
    /// </summary>
    private Result CheckBusinessAccess(Business business)
    {
        var role = currentUser.GetRole();
        if (role == AppRoles.Admin)
            return Result.Success();

        var userId = currentUser.GetUserId();
        if (userId != 0 && business.OwnerUserId == userId)
            return Result.Success();

        return Result.Failure("You do not have access to subscriptions for this business.", ErrorCodes.Forbidden);
    }

    private static SubscriptionDto ToDto(Subscription s, string planName, string planCode) => new(
        s.Id, s.BusinessId, s.SubscriptionPlanId, planName, planCode,
        s.StartDateUtc, s.EndDateUtc, s.Status, s.PaymentStatus, s.AutoRenew,
        s.ExternalReference, s.PaymentReference, s.ActivatedAtUtc, s.CreatedAtUtc);
}
