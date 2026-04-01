using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;

namespace PersianHub.API.Interfaces.Layer2Core;

public interface ISubscriptionPlanService
{
    Task<Result<IReadOnlyList<SubscriptionPlanListItemDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<SubscriptionPlanListItemDto>>> GetActiveAsync(CancellationToken ct = default);
    Task<Result<SubscriptionPlanDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<SubscriptionPlanDto>> GetByCodeAsync(string code, CancellationToken ct = default);
}
