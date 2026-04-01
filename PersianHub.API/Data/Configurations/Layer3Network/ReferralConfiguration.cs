using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer3Network;

namespace PersianHub.API.Data.Configurations.Layer3Network;

public class ReferralConfiguration : IEntityTypeConfiguration<Referral>
{
    public void Configure(EntityTypeBuilder<Referral> builder)
    {
        builder.ToTable("Referrals");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Status).IsRequired();
        builder.Property(r => r.RewardStatus).IsRequired();
        builder.Property(r => r.CreatedAtUtc).IsRequired();
        builder.HasOne(r => r.ReferrerUser).WithMany().HasForeignKey(r => r.ReferrerUserId).OnDelete(DeleteBehavior.Restrict);
        builder.Property(r => r.UpdatedAtUtc).IsRequired();
        builder.HasOne(r => r.ReferredUser).WithMany().HasForeignKey(r => r.ReferredUserId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);
        builder.HasOne(r => r.ReferralCode).WithMany(rc => rc.Referrals).HasForeignKey(r => r.ReferralCodeId).OnDelete(DeleteBehavior.SetNull);
    }
}
