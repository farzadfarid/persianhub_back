namespace PersianHub.API.Entities.Layer2Core;

public class BusinessImage
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsCover { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    // Navigation
    public Business Business { get; set; } = null!;
}
