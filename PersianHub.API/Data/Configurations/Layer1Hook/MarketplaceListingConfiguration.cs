using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer1Hook;

namespace PersianHub.API.Data.Configurations.Layer1Hook;

public class MarketplaceListingConfiguration : IEntityTypeConfiguration<MarketplaceListing>
{
    public void Configure(EntityTypeBuilder<MarketplaceListing> builder)
    {
        builder.ToTable("MarketplaceListings");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Title).IsRequired().HasMaxLength(300);
        builder.Property(l => l.Slug).IsRequired().HasMaxLength(300);
        builder.HasIndex(l => l.Slug).IsUnique();
        builder.Property(l => l.Description).HasMaxLength(3000);
        builder.Property(l => l.Category).HasMaxLength(100);
        builder.Property(l => l.Price).HasPrecision(18, 2);
        builder.Property(l => l.Currency).HasMaxLength(3);
        builder.Property(l => l.Condition).HasMaxLength(50);
        builder.Property(l => l.ContactPhoneNumber).HasMaxLength(20);
        builder.Property(l => l.ContactEmail).HasMaxLength(256);
        builder.Property(l => l.PrimaryImageUrl).HasMaxLength(500);
        builder.Property(l => l.CreatedAtUtc).IsRequired();
        builder.Property(l => l.UpdatedAtUtc).IsRequired();
        builder.HasOne(l => l.AppUser).WithMany().HasForeignKey(l => l.AppUserId).OnDelete(DeleteBehavior.Cascade);
    }
}
