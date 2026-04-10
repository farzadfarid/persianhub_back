using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Admin;

public record CreateSubscriptionPlanAdminDto(
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

public record UpdateSubscriptionPlanAdminDto(
    string Name,
    string? NameFa,
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
    int DisplayOrder
);
