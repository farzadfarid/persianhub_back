using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class InteractionConfiguration : IEntityTypeConfiguration<Interaction>
{
    public void Configure(EntityTypeBuilder<Interaction> builder)
    {
        builder.ToTable("Interactions");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.InteractionType).IsRequired();
        builder.Property(i => i.Metadata).HasMaxLength(1000);
        builder.Property(i => i.CreatedAtUtc).IsRequired();
        builder.HasIndex(i => i.BusinessId);
        builder.HasIndex(i => i.AppUserId);
        builder.HasIndex(i => i.CreatedAtUtc);
        builder.HasOne(i => i.Business)
            .WithMany()
            .HasForeignKey(i => i.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(i => i.AppUser)
            .WithMany()
            .HasForeignKey(i => i.AppUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
