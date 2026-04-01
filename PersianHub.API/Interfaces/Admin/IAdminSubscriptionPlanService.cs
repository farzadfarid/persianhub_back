using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Layer2Core;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminSubscriptionPlanService
{
    Task<Result<SubscriptionPlanDto>> CreateAsync(CreateSubscriptionPlanAdminDto dto, CancellationToken ct = default);
    Task<Result<SubscriptionPlanDto>> UpdateAsync(int id, UpdateSubscriptionPlanAdminDto dto, CancellationToken ct = default);
    Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default);
}
