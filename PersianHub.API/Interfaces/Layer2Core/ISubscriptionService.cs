using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;

namespace PersianHub.API.Interfaces.Layer2Core;

public interface ISubscriptionService
{
    /// <summary>
    /// Creates a new subscription for a business.
    /// Returns Conflict if the business already has an Active subscription — cancel it first.
    /// </summary>
    Task<Result<SubscriptionDto>> CreateAsync(CreateSubscriptionRequestDto request, CancellationToken ct = default);

    Task<Result<SubscriptionDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<SubscriptionDto>>> GetForBusinessAsync(int businessId, CancellationToken ct = default);

    /// <summary>Returns the current Active subscription for a business, or null if none.</summary>
    Task<Result<SubscriptionDto?>> GetActiveForBusinessAsync(int businessId, CancellationToken ct = default);

    Task<Result> CancelAsync(int subscriptionId, CancellationToken ct = default);

    /// <summary>Returns all subscriptions across all businesses. Admin role required — enforced in service.</summary>
    Task<Result<IReadOnlyList<SubscriptionDto>>> GetAllAsync(CancellationToken ct = default);
}
