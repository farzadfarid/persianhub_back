namespace PersianHub.API.Entities.Layer2Core;

public class BusinessTag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameFa { get; set; }
    public string Slug { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    // Navigation
    public ICollection<Business> Businesses { get; set; } = [];
}
