using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Admin;

/// <summary>
/// Payment visibility is backed by the Subscription entity.
/// There is no standalone Payment table — PaymentStatus, PaymentReference,
/// ExternalReference, and ActivatedAtUtc live on Subscription.
/// </summary>
public record AdminPaymentListItemDto(
    int SubscriptionId,
    int BusinessId,
    string? BusinessName,
    int SubscriptionPlanId,
    string? PlanCode,
    SubscriptionStatus SubscriptionStatus,
    PaymentStatus PaymentStatus,
    string? PaymentReference,
    string? ExternalReference,
    DateTime? ActivatedAtUtc,
    DateTime StartDateUtc,
    DateTime? EndDateUtc,
    DateTime CreatedAtUtc
);

public record AdminPaymentDetailDto(
    int SubscriptionId,
    int BusinessId,
    string? BusinessName,
    int SubscriptionPlanId,
    string? PlanCode,
    string? PlanName,
    SubscriptionStatus SubscriptionStatus,
    bool AutoRenew,
    PaymentStatus PaymentStatus,
    string? PaymentReference,
    string? ExternalReference,
    DateTime? ActivatedAtUtc,
    DateTime StartDateUtc,
    DateTime? EndDateUtc,
    DateTime CreatedAtUtc
);
