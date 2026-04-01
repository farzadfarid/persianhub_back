using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class BusinessTagConfiguration : IEntityTypeConfiguration<BusinessTag>
{
    public void Configure(EntityTypeBuilder<BusinessTag> builder)
    {
        builder.ToTable("BusinessTags");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Slug).IsRequired().HasMaxLength(100);
        builder.HasIndex(t => t.Slug).IsUnique();
    }
}
