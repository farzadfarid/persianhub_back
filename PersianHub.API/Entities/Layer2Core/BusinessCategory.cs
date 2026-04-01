namespace PersianHub.API.Entities.Layer2Core;

public class BusinessCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }

    // Navigation
    public ICollection<Business> Businesses { get; set; } = [];
}
