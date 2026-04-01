namespace PersianHub.API.DTOs.Layer2Core;

public record BusinessCategoryDto(
    int Id,
    string Name,
    string Slug,
    string? Description,
    int DisplayOrder,
    bool IsActive
);

public record UpsertBusinessCategoryDto(
    string Name,
    string? Slug,
    string? Description,
    int DisplayOrder,
    bool IsActive
);
