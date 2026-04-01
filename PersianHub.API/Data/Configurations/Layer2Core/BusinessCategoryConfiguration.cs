using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Data.Configurations.Layer2Core;

public class BusinessCategoryConfiguration : IEntityTypeConfiguration<BusinessCategory>
{
    public void Configure(EntityTypeBuilder<BusinessCategory> builder)
    {
        builder.ToTable("BusinessCategories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Slug).IsRequired().HasMaxLength(100);
        builder.HasIndex(c => c.Slug).IsUnique();
        builder.Property(c => c.Description).HasMaxLength(500);

        // Initial business categories relevant to the Iranian community in Gothenburg.
        builder.HasData(
            new BusinessCategory { Id = 1,  Name = "Restaurant & Café",         Slug = "restaurant-cafe",         DisplayOrder = 1,  IsActive = true },
            new BusinessCategory { Id = 2,  Name = "Grocery & Food Market",     Slug = "grocery-food-market",     DisplayOrder = 2,  IsActive = true },
            new BusinessCategory { Id = 3,  Name = "Bakery & Sweets",           Slug = "bakery-sweets",           DisplayOrder = 3,  IsActive = true },
            new BusinessCategory { Id = 4,  Name = "Beauty & Hair Salon",       Slug = "beauty-hair-salon",       DisplayOrder = 4,  IsActive = true },
            new BusinessCategory { Id = 5,  Name = "Healthcare & Medical",      Slug = "healthcare-medical",      DisplayOrder = 5,  IsActive = true },
            new BusinessCategory { Id = 6,  Name = "Legal & Immigration",       Slug = "legal-immigration",       DisplayOrder = 6,  IsActive = true },
            new BusinessCategory { Id = 7,  Name = "Financial Services",        Slug = "financial-services",      DisplayOrder = 7,  IsActive = true },
            new BusinessCategory { Id = 8,  Name = "Real Estate",               Slug = "real-estate",             DisplayOrder = 8,  IsActive = true },
            new BusinessCategory { Id = 9,  Name = "Education & Tutoring",      Slug = "education-tutoring",      DisplayOrder = 9,  IsActive = true },
            new BusinessCategory { Id = 10, Name = "Translation & Interpretation", Slug = "translation-interpretation", DisplayOrder = 10, IsActive = true },
            new BusinessCategory { Id = 11, Name = "Clothing & Fashion",        Slug = "clothing-fashion",        DisplayOrder = 11, IsActive = true },
            new BusinessCategory { Id = 12, Name = "Electronics & IT",          Slug = "electronics-it",          DisplayOrder = 12, IsActive = true },
            new BusinessCategory { Id = 13, Name = "Travel Agency",             Slug = "travel-agency",           DisplayOrder = 13, IsActive = true },
            new BusinessCategory { Id = 14, Name = "Photography & Media",       Slug = "photography-media",       DisplayOrder = 14, IsActive = true },
            new BusinessCategory { Id = 15, Name = "Fitness & Sports",          Slug = "fitness-sports",          DisplayOrder = 15, IsActive = true },
            new BusinessCategory { Id = 16, Name = "Home Services & Renovation",Slug = "home-services-renovation",DisplayOrder = 16, IsActive = true },
            new BusinessCategory { Id = 17, Name = "Events & Entertainment",    Slug = "events-entertainment",    DisplayOrder = 17, IsActive = true },
            new BusinessCategory { Id = 18, Name = "Religious & Cultural",      Slug = "religious-cultural",      DisplayOrder = 18, IsActive = true },
            new BusinessCategory { Id = 19, Name = "Other",                     Slug = "other",                   DisplayOrder = 99, IsActive = true }
        );
    }
}
