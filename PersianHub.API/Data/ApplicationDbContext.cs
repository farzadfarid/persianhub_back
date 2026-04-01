using Microsoft.EntityFrameworkCore;
using PersianHub.API.Entities.Common;
using PersianHub.API.Entities.Layer1Hook;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Entities.Layer3Network;

namespace PersianHub.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    // Common
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    // Layer 1 — Hook (Retention Engine)
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventCategory> EventCategories => Set<EventCategory>();
    public DbSet<EventBookmark> EventBookmarks => Set<EventBookmark>();
    public DbSet<Deal> Deals => Set<Deal>();
    public DbSet<DealCategory> DealCategories => Set<DealCategory>();
    public DbSet<DealBookmark> DealBookmarks => Set<DealBookmark>();
    public DbSet<DailyOffer> DailyOffers => Set<DailyOffer>();
    public DbSet<MarketplaceListing> MarketplaceListings => Set<MarketplaceListing>();
    public DbSet<ListingBookmark> ListingBookmarks => Set<ListingBookmark>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<SavedSearch> SavedSearches => Set<SavedSearch>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<UserActivity> UserActivities => Set<UserActivity>();

    // Layer 2 — Core (Monetization Engine)
    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<BusinessCategory> BusinessCategories => Set<BusinessCategory>();
    public DbSet<BusinessImage> BusinessImages => Set<BusinessImage>();
    public DbSet<BusinessWorkingHour> BusinessWorkingHours => Set<BusinessWorkingHour>();
    public DbSet<BusinessTag> BusinessTags => Set<BusinessTag>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ReviewReaction> ReviewReactions => Set<ReviewReaction>();
    public DbSet<ContactRequest> ContactRequests => Set<ContactRequest>();
    public DbSet<Interaction> Interactions => Set<Interaction>();
    public DbSet<FeaturedPlacement> FeaturedPlacements => Set<FeaturedPlacement>();
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<BusinessClaimRequest> BusinessClaimRequests => Set<BusinessClaimRequest>();

    // Layer 3 — Network Effect (Growth Engine)
    public DbSet<ReferralCode> ReferralCodes => Set<ReferralCode>();
    public DbSet<Referral> Referrals => Set<Referral>();
    public DbSet<Invite> Invites => Set<Invite>();
    public DbSet<InviteReward> InviteRewards => Set<InviteReward>();
    public DbSet<BusinessSuggestion> BusinessSuggestions => Set<BusinessSuggestion>();
    public DbSet<ShareLog> ShareLogs => Set<ShareLog>();
    public DbSet<CommunityPost> CommunityPosts => Set<CommunityPost>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Reaction> Reactions => Set<Reaction>();
    public DbSet<Report> Reports => Set<Report>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
