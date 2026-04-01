using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class BusinessConfiguration : IEntityTypeConfiguration<Business>
{
    public void Configure(EntityTypeBuilder<Business> builder)
    {
        builder.ToTable("Businesses");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).IsRequired().HasMaxLength(200);
        builder.Property(b => b.Slug).IsRequired().HasMaxLength(200);
        builder.HasIndex(b => b.Slug).IsUnique();
        builder.Property(b => b.Description).HasMaxLength(2000);
        builder.Property(b => b.PhoneNumber).HasMaxLength(20);
        builder.Property(b => b.Email).HasMaxLength(256);
        builder.Property(b => b.Website).HasMaxLength(500);
        builder.Property(b => b.InstagramUrl).HasMaxLength(500);
        builder.Property(b => b.TelegramUrl).HasMaxLength(500);
        builder.Property(b => b.WhatsAppNumber).HasMaxLength(20);
        builder.Property(b => b.AddressLine).HasMaxLength(300);
        builder.Property(b => b.City).HasMaxLength(100);
        builder.Property(b => b.Region).HasMaxLength(100);
        builder.Property(b => b.PostalCode).HasMaxLength(20);
        builder.Property(b => b.Country).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Latitude).HasPrecision(10, 7);
        builder.Property(b => b.Longitude).HasPrecision(10, 7);
        builder.Property(b => b.CreatedAtUtc).IsRequired();
        builder.Property(b => b.UpdatedAtUtc).IsRequired();
        builder.HasOne(b => b.OwnerUser).WithMany(u => u.Businesses).HasForeignKey(b => b.OwnerUserId).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(b => b.Categories).WithMany(c => c.Businesses).UsingEntity("BusinessCategoryMappings");
        builder.HasMany(b => b.Tags).WithMany(t => t.Businesses).UsingEntity("BusinessTagMappings");
    }
}
