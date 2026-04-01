using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class BusinessImageConfiguration : IEntityTypeConfiguration<BusinessImage>
{
    public void Configure(EntityTypeBuilder<BusinessImage> builder)
    {
        builder.ToTable("BusinessImages");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ImageUrl).IsRequired().HasMaxLength(500);
        builder.Property(i => i.AltText).HasMaxLength(300);
        builder.Property(i => i.CreatedAtUtc).IsRequired();
        builder.HasOne(i => i.Business).WithMany(b => b.Images).HasForeignKey(i => i.BusinessId).OnDelete(DeleteBehavior.Cascade);
    }
}
