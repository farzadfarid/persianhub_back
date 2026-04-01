using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.ToTable("SubscriptionPlans");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Code).IsRequired().HasMaxLength(50);
        builder.HasIndex(p => p.Code).IsUnique();
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.Price).IsRequired().HasPrecision(18, 2);
        builder.Property(p => p.Currency).IsRequired().HasMaxLength(3);
        builder.Property(p => p.BillingCycle).IsRequired();
        builder.Property(p => p.CreatedAtUtc).IsRequired();
        builder.Property(p => p.UpdatedAtUtc).IsRequired();

        // Seed data — prices are placeholders, confirm with product owner before launch.
        var now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        builder.HasData(
            new SubscriptionPlan { Id = 1, Name = "Free", Code = "FREE", Description = "Basic listing with limited visibility.", Price = 0m, Currency = "SEK", BillingCycle = SubscriptionBillingCycle.Monthly, MaxImages = 3, CanBeFeatured = false, PriorityInSearch = false, AllowsDeals = false, AllowsAnalytics = false, IsActive = true, DisplayOrder = 1, CreatedAtUtc = now, UpdatedAtUtc = now },
            new SubscriptionPlan { Id = 2, Name = "Basic", Code = "BASIC", Description = "Enhanced listing with improved visibility and core features.", Price = 299m, Currency = "SEK", BillingCycle = SubscriptionBillingCycle.Monthly, MaxImages = 10, CanBeFeatured = false, PriorityInSearch = false, AllowsDeals = true, AllowsAnalytics = false, IsActive = true, DisplayOrder = 2, CreatedAtUtc = now, UpdatedAtUtc = now },
            new SubscriptionPlan { Id = 3, Name = "Premium", Code = "PREMIUM", Description = "Full platform access with featured listing and priority search.", Price = 699m, Currency = "SEK", BillingCycle = SubscriptionBillingCycle.Monthly, MaxImages = 30, CanBeFeatured = true, PriorityInSearch = true, AllowsDeals = true, AllowsAnalytics = true, IsActive = true, DisplayOrder = 3, CreatedAtUtc = now, UpdatedAtUtc = now }
        );
    }
}
