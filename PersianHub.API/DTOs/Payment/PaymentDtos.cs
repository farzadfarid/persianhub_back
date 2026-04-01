using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.DTOs.Payment;

/// <summary>Request to initiate a payment for a subscription plan.</summary>
public record CreatePaymentRequestDto(
    int BusinessId,
    int SubscriptionPlanId,
    bool AutoRenew,
    /// <summary>Full URL the gateway will redirect back to after payment (e.g. https://persianhub.se/payment/callback).</summary>
    string CallbackUrl
);

/// <summary>Returned when a payment request is successfully created with the gateway.</summary>
public record PaymentInitiatedDto(
    int SubscriptionId,
    /// <summary>Gateway-assigned identifier for this payment session. Stored as ExternalReference on the subscription.</summary>
    string Authority,
    /// <summary>URL the client must redirect the user to for payment.</summary>
    string PaymentUrl
);

/// <summary>Result after verifying a payment callback from the gateway.</summary>
public record PaymentResultDto(
    int SubscriptionId,
    bool Success,
    PaymentStatus PaymentStatus,
    /// <summary>Gateway transaction/reference id — only set on successful payment.</summary>
    string? PaymentReference,
    string Message
);
