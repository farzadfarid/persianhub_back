using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer3Network;

namespace PersianHub.API.Data.Configurations.Layer3Network;

public class CommunityPostConfiguration : IEntityTypeConfiguration<CommunityPost>
{
    public void Configure(EntityTypeBuilder<CommunityPost> builder)
    {
        builder.ToTable("CommunityPosts");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Title).HasMaxLength(300);
        builder.Property(p => p.Body).IsRequired().HasMaxLength(5000);
        builder.Property(p => p.PostType).IsRequired();
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.CreatedAtUtc).IsRequired();
        builder.Property(p => p.UpdatedAtUtc).IsRequired();
        builder.HasOne(p => p.AppUser).WithMany().HasForeignKey(p => p.AppUserId).OnDelete(DeleteBehavior.Cascade);
    }
}
