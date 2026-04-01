using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersianHub.API.Entities.Layer1Hook;

namespace PersianHub.API.Data.Configurations.Layer1Hook;

public class EventCategoryConfiguration : IEntityTypeConfiguration<EventCategory>
{
    public void Configure(EntityTypeBuilder<EventCategory> builder)
    {
        builder.ToTable("EventCategories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Slug).IsRequired().HasMaxLength(100);
        builder.HasIndex(c => c.Slug).IsUnique();
        builder.Property(c => c.Description).HasMaxLength(500);

        // Initial event categories for the Iranian community platform.
        builder.HasData(
            new EventCategory { Id = 1,  Name = "Community Gathering",  Slug = "community-gathering",  DisplayOrder = 1,  IsActive = true },
            new EventCategory { Id = 2,  Name = "Cultural Festival",    Slug = "cultural-festival",    DisplayOrder = 2,  IsActive = true },
            new EventCategory { Id = 3,  Name = "Music & Concert",      Slug = "music-concert",        DisplayOrder = 3,  IsActive = true },
            new EventCategory { Id = 4,  Name = "Sports Event",         Slug = "sports-event",         DisplayOrder = 4,  IsActive = true },
            new EventCategory { Id = 5,  Name = "Educational Workshop", Slug = "educational-workshop", DisplayOrder = 5,  IsActive = true },
            new EventCategory { Id = 6,  Name = "Business Networking",  Slug = "business-networking",  DisplayOrder = 6,  IsActive = true },
            new EventCategory { Id = 7,  Name = "Religious Event",      Slug = "religious-event",      DisplayOrder = 7,  IsActive = true },
            new EventCategory { Id = 8,  Name = "Art Exhibition",       Slug = "art-exhibition",       DisplayOrder = 8,  IsActive = true },
            new EventCategory { Id = 9,  Name = "Food & Cooking",       Slug = "food-cooking",         DisplayOrder = 9,  IsActive = true },
            new EventCategory { Id = 10, Name = "Health & Wellness",    Slug = "health-wellness",      DisplayOrder = 10, IsActive = true },
            new EventCategory { Id = 11, Name = "Other",                Slug = "other",                DisplayOrder = 99, IsActive = true }
        );
    }
}
