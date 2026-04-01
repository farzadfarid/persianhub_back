using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer1Hook;

namespace PersianHub.API.Data.Configurations.Layer1Hook;

public class EventBookmarkConfiguration : IEntityTypeConfiguration<EventBookmark>
{
    public void Configure(EntityTypeBuilder<EventBookmark> builder)
    {
        builder.ToTable("EventBookmarks");
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => new { b.AppUserId, b.EventId }).IsUnique();
        builder.HasOne(b => b.AppUser).WithMany().HasForeignKey(b => b.AppUserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(b => b.Event).WithMany(e => e.Bookmarks).HasForeignKey(b => b.EventId).OnDelete(DeleteBehavior.Cascade);
    }
}
