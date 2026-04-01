using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class BusinessWorkingHourConfiguration : IEntityTypeConfiguration<BusinessWorkingHour>
{
    public void Configure(EntityTypeBuilder<BusinessWorkingHour> builder)
    {
        builder.ToTable("BusinessWorkingHours");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.DayOfWeek).IsRequired();
        builder.HasOne(w => w.Business).WithMany(b => b.WorkingHours).HasForeignKey(w => w.BusinessId).OnDelete(DeleteBehavior.Cascade);
    }
}
