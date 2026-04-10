namespace PersianHub.API.DTOs.Layer1Hook;

public record DealCategoryDto(int Id, string Name, string? NameFa, string Slug, string? Description, string? DescriptionFa, int DisplayOrder, bool IsActive);

public record UpsertDealCategoryDto(string Name, string? NameFa, string? Slug, string? Description, string? DescriptionFa, int DisplayOrder, bool IsActive);
