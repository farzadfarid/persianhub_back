using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Admin;

public record CreateSubscriptionPlanAdminDto(
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

public record UpdateSubscriptionPlanAdminDto(
    string Name,
    string? Description,
    decimal Price,
    string Currency,
    SubscriptionBillingCycle BillingCycle,
    int MaxImages,
    bool CanBeFeatured,
    bool PriorityInSearch,
    bool AllowsDeals,
    bool AllowsAnalytics,
    int DisplayOrder
);
