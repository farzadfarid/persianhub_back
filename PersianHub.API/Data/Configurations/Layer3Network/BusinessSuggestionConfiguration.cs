using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer3Network;

namespace PersianHub.API.Data.Configurations.Layer3Network;

public class BusinessSuggestionConfiguration : IEntityTypeConfiguration<BusinessSuggestion>
{
    public void Configure(EntityTypeBuilder<BusinessSuggestion> builder)
    {
        builder.ToTable("BusinessSuggestions");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.BusinessName).IsRequired().HasMaxLength(200);
        builder.Property(s => s.CategoryText).HasMaxLength(100);
        builder.Property(s => s.PhoneNumber).HasMaxLength(20);
        builder.Property(s => s.Email).HasMaxLength(256);
        builder.Property(s => s.Website).HasMaxLength(500);
        builder.Property(s => s.AddressLine).HasMaxLength(300);
        builder.Property(s => s.City).HasMaxLength(100);
        builder.Property(s => s.Description).HasMaxLength(2000);
        builder.Property(s => s.Status).IsRequired();
        builder.Property(s => s.CreatedAtUtc).IsRequired();
        builder.HasOne(s => s.SuggestedByUser).WithMany().HasForeignKey(s => s.SuggestedByUserId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(s => s.ReviewedByUser).WithMany().HasForeignKey(s => s.ReviewedByUserId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}
