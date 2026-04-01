using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Layer2Core;

public record CreateSubscriptionRequestDto(
    int BusinessId,
    int SubscriptionPlanId,
    bool AutoRenew
    // StartDateUtc and EndDateUtc are set server-side from the plan's BillingCycle.
    // ExternalReference is populated by the future payment gateway webhook.
);

public record SubscriptionDto(
    int Id,
    int BusinessId,
    int SubscriptionPlanId,
    string PlanName,
    string PlanCode,
    DateTime StartDateUtc,
    DateTime? EndDateUtc,
    SubscriptionStatus Status,
    PaymentStatus PaymentStatus,
    bool AutoRenew,
    string? ExternalReference,
    string? PaymentReference,
    DateTime? ActivatedAtUtc,
    DateTime CreatedAtUtc
);

public record BusinessSubscriptionSummaryDto(
    int BusinessId,
    SubscriptionDto? ActiveSubscription,
    int TotalSubscriptions
);
