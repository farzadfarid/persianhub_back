using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer3Network;

namespace PersianHub.API.Data.Configurations.Layer3Network;

public class InviteRewardConfiguration : IEntityTypeConfiguration<InviteReward>
{
    public void Configure(EntityTypeBuilder<InviteReward> builder)
    {
        builder.ToTable("InviteRewards");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.RewardValue).HasPrecision(18, 2);
        builder.Property(r => r.Currency).HasMaxLength(3);
        builder.Property(r => r.Status).IsRequired();
        builder.Property(r => r.CreatedAtUtc).IsRequired();
        builder.HasOne(r => r.AppUser).WithMany().HasForeignKey(r => r.AppUserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(r => r.Referral).WithMany().HasForeignKey(r => r.ReferralId).OnDelete(DeleteBehavior.SetNull);
    }
}
