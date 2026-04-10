namespace PersianHub.API.Entities.Layer1Hook;

public class EventCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameFa { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DescriptionFa { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}
