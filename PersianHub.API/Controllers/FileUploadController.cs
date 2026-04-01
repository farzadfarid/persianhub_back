using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersianHub.API.Controllers;

[ApiController]
[Route("api/v1/files")]
[Authorize]
public sealed class FileUploadController(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    private static readonly HashSet<string> AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

    /// <summary>Upload an image file. Returns the public URL of the uploaded file.</summary>
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(UploadResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "No file provided." });

        if (file.Length > MaxFileSizeBytes)
            return BadRequest(new { error = "File size exceeds the 5 MB limit." });

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(ext))
            return BadRequest(new { error = "Only image files (jpg, png, webp, gif) are allowed." });

        var uploadsPath = Path.Combine(env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsPath);

        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadsPath, fileName);

        await using var stream = System.IO.File.Create(filePath);
        await file.CopyToAsync(stream, ct);

        return Ok(new UploadResultDto(fileName, ext.TrimStart('.')));
    }
}

/// <param name="FileName">e.g. "3f2a1b4c.jpg" — store this in the DB</param>
/// <param name="FileType">e.g. "jpg"</param>
public record UploadResultDto(string FileName, string FileType);
