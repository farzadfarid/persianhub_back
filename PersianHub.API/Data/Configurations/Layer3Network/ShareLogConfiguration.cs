using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer3Network;

namespace PersianHub.API.Data.Configurations.Layer3Network;

public class ShareLogConfiguration : IEntityTypeConfiguration<ShareLog>
{
    public void Configure(EntityTypeBuilder<ShareLog> builder)
    {
        builder.ToTable("ShareLogs");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.ShareType).IsRequired();
        builder.Property(s => s.Channel).IsRequired();
        builder.Property(s => s.ReferenceType).IsRequired();
        builder.HasIndex(s => s.CreatedAtUtc);
        builder.Property(s => s.CreatedAtUtc).IsRequired();
        builder.HasOne(s => s.AppUser).WithMany().HasForeignKey(s => s.AppUserId).OnDelete(DeleteBehavior.SetNull);
    }
}
