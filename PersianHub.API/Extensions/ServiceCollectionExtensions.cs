using Microsoft.AspNetCore.Identity;
using PersianHub.API.Auth;
using PersianHub.API.Common;
using PersianHub.API.Entities.Common;
using PersianHub.API.Interfaces;
using PersianHub.API.Interfaces.Admin;
using PersianHub.API.Interfaces.Layer1Hook;
using PersianHub.API.Interfaces.Layer2Core;
using PersianHub.API.Interfaces.Layer3Network;
using PersianHub.API.Services;
using PersianHub.API.Services.Admin;
using PersianHub.API.Services.Layer1Hook;
using PersianHub.API.Services.Layer2Core;
using PersianHub.API.Services.Layer3Network;

namespace PersianHub.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Audit
        services.AddScoped<IAuditLogService, AuditLogService>();

        // Payment
        services.AddScoped<IPaymentService, PaymentService>();

        // Search / Discovery
        services.AddScoped<ISearchService, SearchService>();

        // Admin
        services.AddScoped<IAdminSubscriptionPlanService, AdminSubscriptionPlanService>();
        services.AddScoped<IAdminBusinessClaimService, AdminBusinessClaimService>();
        services.AddScoped<IAdminBusinessSuggestionService, AdminBusinessSuggestionService>();

        // Auth
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<PasswordHasher<AppUser>>();

        // Common
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        // Layer 2 — Core (Monetization Engine)
        services.AddScoped<IBusinessService, BusinessService>();
        services.AddScoped<IBusinessImageService, BusinessImageService>();
        services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IBusinessCategoryService, BusinessCategoryService>();
        services.AddScoped<IContactRequestService, ContactRequestService>();
        services.AddScoped<IInteractionService, InteractionService>();

        // Layer 1 — Hook (Retention Engine)
        services.AddScoped<IDailyOfferService, DailyOfferService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IUserActivityService, UserActivityService>();

        // Layer 3 — Network Effect (Growth Engine)
        services.AddScoped<IReferralService, ReferralService>();
        services.AddScoped<IInviteService, InviteService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IBusinessSuggestionService, BusinessSuggestionService>();
        services.AddScoped<IShareLogService, ShareLogService>();

        return services;
    }
}
