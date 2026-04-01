using PersianHub.API.Common;
using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.Entities.Layer2Core;

public class SubscriptionPlan : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "SEK";
    public SubscriptionBillingCycle BillingCycle { get; set; }
    public int MaxImages { get; set; }
    public bool CanBeFeatured { get; set; }
    public bool PriorityInSearch { get; set; }
    public bool AllowsDeals { get; set; }
    public bool AllowsAnalytics { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }

    // Navigation
    public ICollection<Subscription> Subscriptions { get; set; } = [];
}
