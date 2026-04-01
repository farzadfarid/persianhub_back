using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer3Network;

namespace PersianHub.API.Data.Configurations.Layer3Network;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Body).IsRequired().HasMaxLength(3000);
        builder.Property(c => c.ReferenceType).IsRequired();
        builder.Property(c => c.Status).IsRequired();
        builder.Property(c => c.CreatedAtUtc).IsRequired();
        builder.Property(c => c.UpdatedAtUtc).IsRequired();
        builder.HasOne(c => c.AppUser).WithMany().HasForeignKey(c => c.AppUserId).OnDelete(DeleteBehavior.Cascade);
    }
}
