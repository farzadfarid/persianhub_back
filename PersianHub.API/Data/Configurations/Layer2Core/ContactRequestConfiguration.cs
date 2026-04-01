using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class ContactRequestConfiguration : IEntityTypeConfiguration<ContactRequest>
{
    public void Configure(EntityTypeBuilder<ContactRequest> builder)
    {
        builder.ToTable("ContactRequests");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(256);
        builder.Property(c => c.PhoneNumber).HasMaxLength(20);
        builder.Property(c => c.Message).HasMaxLength(3000);
        builder.Property(c => c.ContactType).IsRequired();
        builder.Property(c => c.Status).IsRequired();
        builder.Property(c => c.IsConverted).IsRequired();
        builder.Property(c => c.Metadata).HasMaxLength(2000);
        builder.Property(c => c.CreatedAtUtc).IsRequired();
        builder.Property(c => c.UpdatedAtUtc).IsRequired();
        builder.HasIndex(c => c.BusinessId);
        builder.HasIndex(c => c.CreatedAtUtc);
        builder.HasOne(c => c.Business).WithMany(b => b.ContactRequests).HasForeignKey(c => c.BusinessId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(c => c.AppUser).WithMany().HasForeignKey(c => c.AppUserId).OnDelete(DeleteBehavior.SetNull);
    }
}
