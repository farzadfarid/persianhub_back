using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer3Network;

namespace PersianHub.API.Data.Configurations.Layer3Network;

public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder.ToTable("Reactions");
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => new { r.AppUserId, r.ReferenceType, r.ReferenceId, r.ReactionType }).IsUnique();
        builder.Property(r => r.ReferenceType).IsRequired();
        builder.Property(r => r.ReactionType).IsRequired();
        builder.Property(r => r.CreatedAtUtc).IsRequired();
        builder.HasOne(r => r.AppUser).WithMany().HasForeignKey(r => r.AppUserId).OnDelete(DeleteBehavior.Cascade);
    }
}
