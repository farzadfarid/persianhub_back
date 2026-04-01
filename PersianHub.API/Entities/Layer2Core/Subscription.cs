using PersianHub.API.Common;
using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.Entities.Layer2Core;

public class Subscription : AuditableEntity
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public int SubscriptionPlanId { get; set; }
    public DateTime StartDateUtc { get; set; }
    public DateTime? EndDateUtc { get; set; }
    public SubscriptionStatus Status { get; set; }
    public bool AutoRenew { get; set; }
    public string? ExternalReference { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public string? PaymentReference { get; set; }
    public DateTime? ActivatedAtUtc { get; set; }

    // Navigation
    public Business Business { get; set; } = null!;
    public SubscriptionPlan SubscriptionPlan { get; set; } = null!;
}
