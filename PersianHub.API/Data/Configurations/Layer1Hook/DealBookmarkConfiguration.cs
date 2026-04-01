using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer1Hook;

namespace PersianHub.API.Data.Configurations.Layer1Hook;

public class DealBookmarkConfiguration : IEntityTypeConfiguration<DealBookmark>
{
    public void Configure(EntityTypeBuilder<DealBookmark> builder)
    {
        builder.ToTable("DealBookmarks");
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => new { b.AppUserId, b.DealId }).IsUnique();
        builder.HasOne(b => b.AppUser).WithMany().HasForeignKey(b => b.AppUserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(b => b.Deal).WithMany(d => d.Bookmarks).HasForeignKey(b => b.DealId).OnDelete(DeleteBehavior.Cascade);
    }
}
