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

        // Admin — Phase 1: Content Visibility
        services.AddScoped<IAdminSubscriptionPlanService, AdminSubscriptionPlanService>();
        services.AddScoped<IAdminBusinessClaimService, AdminBusinessClaimService>();
        services.AddScoped<IAdminBusinessSuggestionService, AdminBusinessSuggestionService>();
        services.AddScoped<IAdminDealService, AdminDealService>();
        services.AddScoped<IAdminDailyOfferService, AdminDailyOfferService>();
        services.AddScoped<IAdminEventService, AdminEventService>();
        services.AddScoped<IAdminReviewService, AdminReviewService>();
        services.AddScoped<IAdminReportService, AdminReportService>();

        // Admin — Phase 2: Monetization and Growth
        services.AddScoped<IAdminPaymentService, AdminPaymentService>();
        services.AddScoped<IAdminReferralCodeService, AdminReferralCodeService>();
        services.AddScoped<IAdminReferralService, AdminReferralService>();
        services.AddScoped<IAdminInviteService, AdminInviteService>();
        services.AddScoped<IAdminInviteRewardService, AdminInviteRewardService>();

        // Admin — Phase 3: Community and Moderation
        services.AddScoped<IAdminCommunityPostService, AdminCommunityPostService>();
        services.AddScoped<IAdminCommentService, AdminCommentService>();
        services.AddScoped<IAdminReactionService, AdminReactionService>();
        services.AddScoped<IAdminReviewReactionService, AdminReviewReactionService>();

        // Admin — Phase 4: System Operations and Observability
        services.AddScoped<IAdminAuditLogService, AdminAuditLogService>();

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
        services.AddScoped<IBusinessTagService, BusinessTagService>();
        services.AddScoped<IContactRequestService, ContactRequestService>();
        services.AddScoped<IInteractionService, InteractionService>();

        // Layer 1 — Hook (Retention Engine)
        services.AddScoped<IDealService, DealService>();
        services.AddScoped<IDailyOfferService, DailyOfferService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IUserActivityService, UserActivityService>();
        services.AddScoped<IDealCategoryService, DealCategoryService>();
        services.AddScoped<IEventCategoryService, EventCategoryService>();

        // Layer 3 — Network Effect (Growth Engine)
        services.AddScoped<IReferralService, ReferralService>();
        services.AddScoped<IInviteService, InviteService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IReviewReactionService, ReviewReactionService>();
        services.AddScoped<IBusinessSuggestionService, BusinessSuggestionService>();
        services.AddScoped<IShareLogService, ShareLogService>();
        services.AddScoped<IReportService, ReportService>();

        return services;
    }
}
