namespace PersianHub.API.DTOs.Layer1Hook;

public record EventCategoryDto(int Id, string Name, string? NameFa, string Slug, string? Description, string? DescriptionFa, int DisplayOrder, bool IsActive);

public record UpsertEventCategoryDto(string Name, string? NameFa, string? Slug, string? Description, string? DescriptionFa, int DisplayOrder, bool IsActive);
