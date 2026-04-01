using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer1Hook;

namespace PersianHub.API.Data.Configurations.Layer1Hook;

public class SavedSearchConfiguration : IEntityTypeConfiguration<SavedSearch>
{
    public void Configure(EntityTypeBuilder<SavedSearch> builder)
    {
        builder.ToTable("SavedSearches");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.QueryText).HasMaxLength(500);
        builder.Property(s => s.FilterJson).HasMaxLength(2000);
        builder.Property(s => s.CreatedAtUtc).IsRequired();
        builder.Property(s => s.UpdatedAtUtc).IsRequired();
        builder.HasOne(s => s.AppUser).WithMany().HasForeignKey(s => s.AppUserId).OnDelete(DeleteBehavior.Cascade);
    }
}
