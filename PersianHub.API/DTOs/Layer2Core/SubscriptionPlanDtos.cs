using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Layer2Core;

public record SubscriptionPlanDto(
    int Id,
    string Name,
    string Code,
    string? Description,
    decimal Price,
    string Currency,
    SubscriptionBillingCycle BillingCycle,
    int MaxImages,
    bool CanBeFeatured,
    bool PriorityInSearch,
    bool AllowsDeals,
    bool AllowsAnalytics,
    bool IsActive,
    int DisplayOrder
);

public record SubscriptionPlanListItemDto(
    int Id,
    string Name,
    string Code,
    decimal Price,
    string Currency,
    SubscriptionBillingCycle BillingCycle,
    bool IsActive,
    int DisplayOrder
);
