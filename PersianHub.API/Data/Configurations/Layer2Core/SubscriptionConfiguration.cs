using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.ExternalReference).HasMaxLength(200);
        builder.Property(s => s.PaymentReference).HasMaxLength(200);
        builder.Property(s => s.PaymentStatus).IsRequired();
        builder.Property(s => s.Status).IsRequired();
        builder.Property(s => s.CreatedAtUtc).IsRequired();
        builder.Property(s => s.UpdatedAtUtc).IsRequired();
        builder.HasOne(s => s.Business).WithMany(b => b.Subscriptions).HasForeignKey(s => s.BusinessId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(s => s.SubscriptionPlan).WithMany(p => p.Subscriptions).HasForeignKey(s => s.SubscriptionPlanId).OnDelete(DeleteBehavior.Restrict);
    }
}
