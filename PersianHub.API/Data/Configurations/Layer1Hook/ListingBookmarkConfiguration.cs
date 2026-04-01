using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer1Hook;

namespace PersianHub.API.Data.Configurations.Layer1Hook;

public class ListingBookmarkConfiguration : IEntityTypeConfiguration<ListingBookmark>
{
    public void Configure(EntityTypeBuilder<ListingBookmark> builder)
    {
        builder.ToTable("ListingBookmarks");
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => new { b.AppUserId, b.MarketplaceListingId }).IsUnique();
        builder.HasOne(b => b.AppUser).WithMany().HasForeignKey(b => b.AppUserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(b => b.MarketplaceListing).WithMany(l => l.Bookmarks).HasForeignKey(b => b.MarketplaceListingId).OnDelete(DeleteBehavior.Restrict);
    }
}
