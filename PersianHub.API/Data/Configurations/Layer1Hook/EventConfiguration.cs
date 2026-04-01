using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer1Hook;

namespace PersianHub.API.Data.Configurations.Layer1Hook;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(300);
        builder.Property(e => e.Slug).IsRequired().HasMaxLength(300);
        builder.HasIndex(e => e.Slug).IsUnique();
        builder.Property(e => e.Description).HasMaxLength(5000);
        builder.Property(e => e.LocationName).HasMaxLength(300);
        builder.Property(e => e.OrganizerName).HasMaxLength(200);
        builder.Property(e => e.OrganizerEmail).HasMaxLength(256);
        builder.Property(e => e.OrganizerPhoneNumber).HasMaxLength(20);
        builder.Property(e => e.CoverImageUrl).HasMaxLength(500);
        builder.Property(e => e.Currency).HasMaxLength(3);
        builder.Property(e => e.Price).HasPrecision(18, 2);
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtUtc).IsRequired();
        builder.HasOne(e => e.CreatedByUser)
            .WithMany()
            .HasForeignKey(e => e.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(e => e.Business)
            .WithMany()
            .HasForeignKey(e => e.BusinessId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
