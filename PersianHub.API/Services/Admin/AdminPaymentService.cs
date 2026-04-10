using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

/// <summary>
/// Payment visibility backed by the Subscription table.
/// No standalone Payment entity exists — PaymentStatus, PaymentReference,
/// ExternalReference, and ActivatedAtUtc are fields on Subscription.
/// </summary>
public sealed class AdminPaymentService(ApplicationDbContext db) : IAdminPaymentService
{
    public async Task<PagedResult<AdminPaymentListItemDto>> GetAllAsync(
        int? businessId, PaymentStatus? paymentStatus,
        DateTime? fromUtc, DateTime? toUtc,
        string? search,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.Subscriptions
            .AsNoTracking()
            .Include(s => s.Business)
            .Include(s => s.SubscriptionPlan)
            .AsQueryable();

        if (businessId.HasValue)
            query = query.Where(s => s.BusinessId == businessId.Value);

        if (paymentStatus.HasValue)
            query = query.Where(s => s.PaymentStatus == paymentStatus.Value);

        if (fromUtc.HasValue)
            query = query.Where(s => s.CreatedAtUtc >= fromUtc.Value);

        if (toUtc.HasValue)
            query = query.Where(s => s.CreatedAtUtc <= toUtc.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(s =>
                (s.PaymentReference != null && s.PaymentReference.Contains(search)) ||
                (s.ExternalReference != null && s.ExternalReference.Contains(search)));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(s => s.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new AdminPaymentListItemDto(
                s.Id, s.BusinessId, s.Business.Name,
                s.SubscriptionPlanId, s.SubscriptionPlan.Code,
                s.Status, s.PaymentStatus,
                s.PaymentReference, s.ExternalReference,
                s.ActivatedAtUtc, s.StartDateUtc, s.EndDateUtc,
                s.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminPaymentListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminPaymentDetailDto>> GetByIdAsync(int subscriptionId, CancellationToken ct)
    {
        var sub = await db.Subscriptions
            .AsNoTracking()
            .Include(s => s.Business)
            .Include(s => s.SubscriptionPlan)
            .FirstOrDefaultAsync(s => s.Id == subscriptionId, ct);

        if (sub is null)
            return Result<AdminPaymentDetailDto>.Failure("Subscription not found.", ErrorCodes.NotFound);

        return Result<AdminPaymentDetailDto>.Success(new AdminPaymentDetailDto(
            sub.Id, sub.BusinessId, sub.Business.Name,
            sub.SubscriptionPlanId, sub.SubscriptionPlan.Code, sub.SubscriptionPlan.Name,
            sub.Status, sub.AutoRenew,
            sub.PaymentStatus, sub.PaymentReference, sub.ExternalReference,
            sub.ActivatedAtUtc, sub.StartDateUtc, sub.EndDateUtc,
            sub.CreatedAtUtc));
    }
}
