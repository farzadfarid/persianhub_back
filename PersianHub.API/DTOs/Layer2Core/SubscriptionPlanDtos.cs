using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Layer2Core;

public record SubscriptionPlanDto(
    int Id,
    string Name,
    string? NameFa,
    string Code,
    string? Description,
    string? DescriptionFa,
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
    string? NameFa,
    string Code,
    decimal Price,
    string Currency,
    SubscriptionBillingCycle BillingCycle,
    bool IsActive,
    int DisplayOrder
);
