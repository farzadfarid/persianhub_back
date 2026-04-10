using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Interfaces;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

public sealed class AdminSubscriptionPlanService(
    ApplicationDbContext db,
    IDateTimeProvider clock,
    IAuditLogService audit) : IAdminSubscriptionPlanService
{
    public async Task<Result<SubscriptionPlanDto>> CreateAsync(CreateSubscriptionPlanAdminDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return Result<SubscriptionPlanDto>.Failure("Plan name is required.", ErrorCodes.ValidationFailed);

        if (string.IsNullOrWhiteSpace(dto.Code))
            return Result<SubscriptionPlanDto>.Failure("Plan code is required.", ErrorCodes.ValidationFailed);

        var code = dto.Code.Trim().ToUpperInvariant();
        var codeExists = await db.SubscriptionPlans.AnyAsync(p => p.Code == code, ct);
        if (codeExists)
            return Result<SubscriptionPlanDto>.Failure($"A plan with code '{code}' already exists.", ErrorCodes.AlreadyExists);

        var now = clock.UtcNow;
        var plan = new SubscriptionPlan
        {
            Name = dto.Name.Trim(),
            NameFa = dto.NameFa?.Trim(),
            Code = code,
            Description = dto.Description?.Trim(),
            DescriptionFa = dto.DescriptionFa?.Trim(),
            Price = dto.Price,
            Currency = string.IsNullOrWhiteSpace(dto.Currency) ? "SEK" : dto.Currency.Trim().ToUpperInvariant(),
            BillingCycle = dto.BillingCycle,
            MaxImages = dto.MaxImages,
            CanBeFeatured = dto.CanBeFeatured,
            PriorityInSearch = dto.PriorityInSearch,
            AllowsDeals = dto.AllowsDeals,
            AllowsAnalytics = dto.AllowsAnalytics,
            IsActive = dto.IsActive,
            DisplayOrder = dto.DisplayOrder,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.SubscriptionPlans.Add(plan);
        await db.SaveChangesAsync(ct);

        await audit.WriteAsync(AuditActions.AdminSubscriptionPlanCreated, "SubscriptionPlan", plan.Id.ToString(),
            new { plan.Code, plan.Name, plan.Price, plan.IsActive }, ct);

        return Result<SubscriptionPlanDto>.Success(ToDto(plan));
    }

    public async Task<Result<SubscriptionPlanDto>> UpdateAsync(int id, UpdateSubscriptionPlanAdminDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return Result<SubscriptionPlanDto>.Failure("Plan name is required.", ErrorCodes.ValidationFailed);

        var plan = await db.SubscriptionPlans.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (plan is null)
            return Result<SubscriptionPlanDto>.Failure($"Subscription plan with id {id} not found.", ErrorCodes.NotFound);

        plan.Name = dto.Name.Trim();
        plan.NameFa = dto.NameFa?.Trim();
        plan.Description = dto.Description?.Trim();
        plan.DescriptionFa = dto.DescriptionFa?.Trim();
        plan.Price = dto.Price;
        plan.Currency = string.IsNullOrWhiteSpace(dto.Currency) ? "SEK" : dto.Currency.Trim().ToUpperInvariant();
        plan.BillingCycle = dto.BillingCycle;
        plan.MaxImages = dto.MaxImages;
        plan.CanBeFeatured = dto.CanBeFeatured;
        plan.PriorityInSearch = dto.PriorityInSearch;
        plan.AllowsDeals = dto.AllowsDeals;
        plan.AllowsAnalytics = dto.AllowsAnalytics;
        plan.DisplayOrder = dto.DisplayOrder;
        plan.UpdatedAtUtc = clock.UtcNow;

        await db.SaveChangesAsync(ct);

        await audit.WriteAsync(AuditActions.AdminSubscriptionPlanUpdated, "SubscriptionPlan", plan.Id.ToString(),
            new { plan.Code, plan.Name, plan.Price }, ct);

        return Result<SubscriptionPlanDto>.Success(ToDto(plan));
    }

    public async Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default)
    {
        var plan = await db.SubscriptionPlans.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (plan is null)
            return Result.Failure($"Subscription plan with id {id} not found.", ErrorCodes.NotFound);

        plan.IsActive = isActive;
        plan.UpdatedAtUtc = clock.UtcNow;
        await db.SaveChangesAsync(ct);

        var action = isActive ? AuditActions.AdminSubscriptionPlanActivated : AuditActions.AdminSubscriptionPlanDeactivated;
        await audit.WriteAsync(action, "SubscriptionPlan", plan.Id.ToString(), new { plan.Code }, ct);

        return Result.Success();
    }

    private static SubscriptionPlanDto ToDto(SubscriptionPlan p) => new(
        p.Id, p.Name, p.NameFa, p.Code, p.Description, p.DescriptionFa, p.Price, p.Currency, p.BillingCycle,
        p.MaxImages, p.CanBeFeatured, p.PriorityInSearch, p.AllowsDeals, p.AllowsAnalytics,
        p.IsActive, p.DisplayOrder);
}
