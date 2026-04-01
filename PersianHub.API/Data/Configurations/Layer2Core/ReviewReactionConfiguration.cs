using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class ReviewReactionConfiguration : IEntityTypeConfiguration<ReviewReaction>
{
    public void Configure(EntityTypeBuilder<ReviewReaction> builder)
    {
        builder.ToTable("ReviewReactions");
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => new { r.ReviewId, r.AppUserId, r.ReactionType }).IsUnique();
        builder.Property(r => r.CreatedAtUtc).IsRequired();
        builder.HasOne(r => r.Review).WithMany(rv => rv.Reactions).HasForeignKey(r => r.ReviewId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(r => r.AppUser).WithMany().HasForeignKey(r => r.AppUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
