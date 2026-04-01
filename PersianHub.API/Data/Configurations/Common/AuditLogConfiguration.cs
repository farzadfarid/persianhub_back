using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Data.Configurations.Common;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.CorrelationId).HasMaxLength(100);
        builder.Property(a => a.Action).IsRequired().HasMaxLength(100);
        builder.Property(a => a.EntityType).IsRequired().HasMaxLength(100);
        builder.Property(a => a.EntityId).HasMaxLength(50);
        builder.Property(a => a.PerformedByRole).HasMaxLength(50);
        builder.Property(a => a.DetailsJson).HasMaxLength(2000);
        builder.Property(a => a.CreatedAtUtc).IsRequired();

        // Query indexes — common access patterns for audit log browsing
        builder.HasIndex(a => a.Action);
        builder.HasIndex(a => a.PerformedByUserId);
        builder.HasIndex(a => new { a.EntityType, a.EntityId });
        builder.HasIndex(a => a.CreatedAtUtc);
    }
}
