using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer1Hook;

namespace PersianHub.API.Data.Configurations.Layer1Hook;

public class UserActivityConfiguration : IEntityTypeConfiguration<UserActivity>
{
    public void Configure(EntityTypeBuilder<UserActivity> builder)
    {
        builder.ToTable("UserActivities");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.ActivityType).IsRequired();
        builder.Property(u => u.Metadata).HasMaxLength(1000);
        builder.Property(u => u.CreatedAtUtc).IsRequired();
        builder.HasIndex(u => u.AppUserId);
        builder.HasIndex(u => u.CreatedAtUtc);
        builder.HasOne(u => u.AppUser)
            .WithMany()
            .HasForeignKey(u => u.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
