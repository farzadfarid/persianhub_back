using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer1Hook;

namespace PersianHub.API.Data.Configurations.Layer1Hook;

public class DealCategoryConfiguration : IEntityTypeConfiguration<DealCategory>
{
    public void Configure(EntityTypeBuilder<DealCategory> builder)
    {
        builder.ToTable("DealCategories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Slug).IsRequired().HasMaxLength(100);
        builder.HasIndex(c => c.Slug).IsUnique();
        builder.Property(c => c.Description).HasMaxLength(500);

        // Initial deal categories for the Iranian community platform.
        builder.HasData(
            new DealCategory { Id = 1,  Name = "Food & Dining",        Slug = "food-dining",         DisplayOrder = 1,  IsActive = true },
            new DealCategory { Id = 2,  Name = "Beauty & Wellness",    Slug = "beauty-wellness",     DisplayOrder = 2,  IsActive = true },
            new DealCategory { Id = 3,  Name = "Shopping & Fashion",   Slug = "shopping-fashion",    DisplayOrder = 3,  IsActive = true },
            new DealCategory { Id = 4,  Name = "Home & Living",        Slug = "home-living",         DisplayOrder = 4,  IsActive = true },
            new DealCategory { Id = 5,  Name = "Education & Training", Slug = "education-training",  DisplayOrder = 5,  IsActive = true },
            new DealCategory { Id = 6,  Name = "Travel & Leisure",     Slug = "travel-leisure",      DisplayOrder = 6,  IsActive = true },
            new DealCategory { Id = 7,  Name = "Entertainment",        Slug = "entertainment",       DisplayOrder = 7,  IsActive = true },
            new DealCategory { Id = 8,  Name = "Health & Medical",     Slug = "health-medical",      DisplayOrder = 8,  IsActive = true },
            new DealCategory { Id = 9,  Name = "Other",                Slug = "other",               DisplayOrder = 99, IsActive = true }
        );
    }
}
