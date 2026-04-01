using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Services.Layer2Core;

public sealed class SubscriptionPlanService(ApplicationDbContext db) : ISubscriptionPlanService
{
    public async Task<Result<IReadOnlyList<SubscriptionPlanListItemDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var plans = await db.SubscriptionPlans
            .AsNoTracking()
            .OrderBy(p => p.DisplayOrder)
            .Select(p => ToListItemDto(p))
            .ToListAsync(ct);

        return Result<IReadOnlyList<SubscriptionPlanListItemDto>>.Success(plans);
    }

    public async Task<Result<IReadOnlyList<SubscriptionPlanListItemDto>>> GetActiveAsync(CancellationToken ct = default)
    {
        var plans = await db.SubscriptionPlans
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayOrder)
            .Select(p => ToListItemDto(p))
            .ToListAsync(ct);

        return Result<IReadOnlyList<SubscriptionPlanListItemDto>>.Success(plans);
    }

    public async Task<Result<SubscriptionPlanDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var plan = await db.SubscriptionPlans.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
        if (plan is null)
            return Result<SubscriptionPlanDto>.Failure($"Subscription plan with id {id} not found.", ErrorCodes.NotFound);

        return Result<SubscriptionPlanDto>.Success(ToDto(plan));
    }

    public async Task<Result<SubscriptionPlanDto>> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result<SubscriptionPlanDto>.Failure("Plan code is required.", ErrorCodes.ValidationFailed);

        var plan = await db.SubscriptionPlans
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Code == code.Trim().ToUpperInvariant(), ct);

        if (plan is null)
            return Result<SubscriptionPlanDto>.Failure($"Subscription plan with code '{code}' not found.", ErrorCodes.NotFound);

        return Result<SubscriptionPlanDto>.Success(ToDto(plan));
    }

    private static SubscriptionPlanDto ToDto(SubscriptionPlan p) => new(
        p.Id, p.Name, p.Code, p.Description, p.Price, p.Currency, p.BillingCycle,
        p.MaxImages, p.CanBeFeatured, p.PriorityInSearch, p.AllowsDeals, p.AllowsAnalytics,
        p.IsActive, p.DisplayOrder);

    private static SubscriptionPlanListItemDto ToListItemDto(SubscriptionPlan p) => new(
        p.Id, p.Name, p.Code, p.Price, p.Currency, p.BillingCycle, p.IsActive, p.DisplayOrder);
}
