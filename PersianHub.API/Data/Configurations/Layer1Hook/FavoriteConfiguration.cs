using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer1Hook;

namespace PersianHub.API.Data.Configurations.Layer1Hook;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("Favorites");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.ReferenceType).IsRequired();
        builder.Property(f => f.ReferenceId).IsRequired();
        builder.Property(f => f.CreatedAtUtc).IsRequired();
        builder.HasIndex(f => new { f.AppUserId, f.ReferenceType, f.ReferenceId }).IsUnique();
        builder.HasIndex(f => f.CreatedAtUtc);
        builder.HasOne(f => f.AppUser)
            .WithMany()
            .HasForeignKey(f => f.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
