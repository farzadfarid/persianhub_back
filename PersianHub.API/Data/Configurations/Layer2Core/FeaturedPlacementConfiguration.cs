using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class FeaturedPlacementConfiguration : IEntityTypeConfiguration<FeaturedPlacement>
{
    public void Configure(EntityTypeBuilder<FeaturedPlacement> builder)
    {
        builder.ToTable("FeaturedPlacements");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.PlacementType).IsRequired();
        builder.Property(f => f.CreatedAtUtc).IsRequired();
        builder.Property(f => f.UpdatedAtUtc).IsRequired();
        builder.HasIndex(f => new { f.IsActive, f.EndsAtUtc });
        builder.HasOne(f => f.Business).WithMany(b => b.FeaturedPlacements).HasForeignKey(f => f.BusinessId).OnDelete(DeleteBehavior.Cascade);
    }
}
