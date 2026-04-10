namespace PersianHub.API.DTOs.Layer2Core;

public record BusinessTagDto(int Id, string Name, string? NameFa, string Slug, bool IsActive);

public record UpsertBusinessTagDto(string Name, string? NameFa, string? Slug, bool IsActive);
