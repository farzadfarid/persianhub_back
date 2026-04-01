using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class BusinessClaimRequestConfiguration : IEntityTypeConfiguration<BusinessClaimRequest>
{
    public void Configure(EntityTypeBuilder<BusinessClaimRequest> builder)
    {
        builder.ToTable("BusinessClaimRequests");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.SubmittedBusinessEmail).HasMaxLength(256);
        builder.Property(c => c.SubmittedPhoneNumber).HasMaxLength(20);
        builder.Property(c => c.Message).HasMaxLength(2000);
        builder.Property(c => c.Status).IsRequired();
        builder.Property(c => c.CreatedAtUtc).IsRequired();
        builder.Property(c => c.UpdatedAtUtc).IsRequired();
        builder.HasOne(c => c.Business).WithMany(b => b.ClaimRequests).HasForeignKey(c => c.BusinessId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(c => c.AppUser).WithMany().HasForeignKey(c => c.AppUserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(c => c.ReviewedByUser).WithMany().HasForeignKey(c => c.ReviewedByUserId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}
