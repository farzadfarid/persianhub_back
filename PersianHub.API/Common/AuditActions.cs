namespace PersianHub.API.Common;

/// <summary>
/// Well-known audit action names used in <see cref="PersianHub.API.Entities.Common.AuditLog"/>.
/// Use these constants instead of inline strings to keep action names consistent and searchable.
/// </summary>
public static class AuditActions
{
    // Auth
    public const string UserRegistered       = "UserRegistered";
    public const string UserLoginSucceeded   = "UserLoginSucceeded";
    public const string UserLoginFailed      = "UserLoginFailed";
    public const string UserRegistrationFailed = "UserRegistrationFailed";

    // Business
    public const string BusinessCreated      = "BusinessCreated";
    public const string BusinessUpdated      = "BusinessUpdated";
    public const string BusinessActivated    = "BusinessActivated";
    public const string BusinessDeactivated  = "BusinessDeactivated";
    public const string BusinessFeatured     = "BusinessFeatured";
    public const string BusinessUnfeatured   = "BusinessUnfeatured";

    // Business Claim
    public const string BusinessClaimApproved = "BusinessClaimApproved";
    public const string BusinessClaimRejected = "BusinessClaimRejected";

    // Business Suggestion
    public const string BusinessSuggestionApproved = "BusinessSuggestionApproved";
    public const string BusinessSuggestionRejected = "BusinessSuggestionRejected";

    // Subscription
    public const string SubscriptionCreated   = "SubscriptionCreated";
    public const string SubscriptionCancelled = "SubscriptionCancelled";
    public const string SubscriptionActivated = "SubscriptionActivated";

    // Payment
    public const string PaymentRequested        = "PaymentRequested";
    public const string PaymentCallbackReceived = "PaymentCallbackReceived";
    public const string PaymentVerified         = "PaymentVerified";
    public const string PaymentFailed           = "PaymentFailed";

    // Admin — Subscription Plans
    public const string AdminSubscriptionPlanCreated     = "AdminSubscriptionPlanCreated";
    public const string AdminSubscriptionPlanUpdated     = "AdminSubscriptionPlanUpdated";
    public const string AdminSubscriptionPlanActivated   = "AdminSubscriptionPlanActivated";
    public const string AdminSubscriptionPlanDeactivated = "AdminSubscriptionPlanDeactivated";

    // Admin — Subscriptions
    public const string AdminSubscriptionForceCancelled = "AdminSubscriptionForceCancelled";
}
