using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer1Hook;

namespace PersianHub.API.Data.Configurations.Layer1Hook;

public class DailyOfferConfiguration : IEntityTypeConfiguration<DailyOffer>
{
    public void Configure(EntityTypeBuilder<DailyOffer> builder)
    {
        builder.ToTable("DailyOffers");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Title).IsRequired().HasMaxLength(300);
        builder.Property(d => d.Slug).IsRequired().HasMaxLength(300);
        builder.HasIndex(d => d.Slug).IsUnique();
        builder.Property(d => d.Description).HasMaxLength(3000);
        builder.Property(d => d.DiscountType).IsRequired();
        builder.Property(d => d.DiscountValue).HasPrecision(18, 2).IsRequired();
        builder.Property(d => d.OriginalPrice).HasPrecision(18, 2);
        builder.Property(d => d.DiscountedPrice).HasPrecision(18, 2);
        builder.Property(d => d.Currency).IsRequired().HasMaxLength(3);
        builder.Property(d => d.CoverImageUrl).HasMaxLength(500);
        builder.Property(d => d.CreatedAtUtc).IsRequired();
        builder.Property(d => d.UpdatedAtUtc).IsRequired();
        builder.HasIndex(d => d.BusinessId);
        builder.HasIndex(d => new { d.IsActive, d.IsPublished });
        builder.HasIndex(d => d.StartsAtUtc);
        builder.HasOne(d => d.Business)
            .WithMany()
            .HasForeignKey(d => d.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
