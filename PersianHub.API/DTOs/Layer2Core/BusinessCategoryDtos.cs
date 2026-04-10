namespace PersianHub.API.DTOs.Layer2Core;

public record BusinessCategoryDto(
    int Id,
    string Name,
    string? NameFa,
    string Slug,
    string? Description,
    string? DescriptionFa,
    int DisplayOrder,
    bool IsActive
);

public record UpsertBusinessCategoryDto(
    string Name,
    string? NameFa,
    string? Slug,
    string? Description,
    string? DescriptionFa,
    int DisplayOrder,
    bool IsActive
);
