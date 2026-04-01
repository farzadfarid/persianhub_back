using System.Text;
using System.Text.RegularExpressions;

namespace PersianHub.API.Common;

/// <summary>
/// Generates URL-safe slugs from text.
/// Note: Persian/Arabic input is preserved as-is in the slug after normalization.
/// For full Persian transliteration support, a dedicated library should be added in a future task.
/// </summary>
public static class SlugHelper
{
    private static readonly Regex MultipleHyphens = new(@"-{2,}", RegexOptions.Compiled);
    private static readonly Regex InvalidChars = new(@"[^a-z0-9\u0600-\u06FF\-]", RegexOptions.Compiled);

    public static string Generate(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var slug = text.Trim().ToLowerInvariant();

        // Replace whitespace and common separators with hyphens
        slug = Regex.Replace(slug, @"[\s/\\|_]+", "-");

        // Remove characters that are not lowercase latin, Persian Unicode block, or hyphens
        slug = InvalidChars.Replace(slug, string.Empty);

        // Collapse multiple consecutive hyphens
        slug = MultipleHyphens.Replace(slug, "-");

        // Trim leading/trailing hyphens
        slug = slug.Trim('-');

        return slug;
    }

    /// <summary>
    /// Makes a slug unique by appending a numeric suffix if the base slug already exists.
    /// </summary>
    public static string MakeUnique(string baseSlug, IEnumerable<string> existingSlugs)
    {
        var existing = new HashSet<string>(existingSlugs, StringComparer.OrdinalIgnoreCase);

        if (!existing.Contains(baseSlug))
            return baseSlug;

        for (int i = 2; i < 1000; i++)
        {
            var candidate = $"{baseSlug}-{i}";
            if (!existing.Contains(candidate))
                return candidate;
        }

        // Fallback — extremely unlikely
        return $"{baseSlug}-{Guid.NewGuid():N}";
    }
}
