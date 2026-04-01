using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Rating).IsRequired();
        builder.Property(r => r.Title).HasMaxLength(200);
        builder.Property(r => r.Comment).HasMaxLength(3000);
        builder.Property(r => r.Status).IsRequired();
        builder.Property(r => r.CreatedAtUtc).IsRequired();
        builder.Property(r => r.UpdatedAtUtc).IsRequired();
        builder.HasOne(r => r.Business).WithMany(b => b.Reviews).HasForeignKey(r => r.BusinessId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(r => r.AppUser).WithMany().HasForeignKey(r => r.AppUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
