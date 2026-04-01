using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer3Network;

namespace PersianHub.API.Data.Configurations.Layer3Network;

public class InviteConfiguration : IEntityTypeConfiguration<Invite>
{
    public void Configure(EntityTypeBuilder<Invite> builder)
    {
        builder.ToTable("Invites");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.InviteeEmail).HasMaxLength(256);
        builder.Property(i => i.InviteePhoneNumber).HasMaxLength(20);
        builder.Property(i => i.Channel).IsRequired();
        builder.Property(i => i.Status).IsRequired();
        builder.Property(i => i.CreatedAtUtc).IsRequired();
        builder.Property(i => i.UpdatedAtUtc).IsRequired();
        builder.HasOne(i => i.InviterUser).WithMany().HasForeignKey(i => i.InviterUserId).OnDelete(DeleteBehavior.Cascade);
    }
}
