using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Layer2Core;

public sealed class BusinessImagesController(IBusinessImageService imageService) : ApiControllerBase
{
    /// <summary>Get all gallery images for a business.</summary>
    [HttpGet("/api/v1/businesses/{businessId:int}/images")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<BusinessImageDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int businessId, CancellationToken ct)
        => MapResult(await imageService.GetByBusinessIdAsync(businessId, ct));

    /// <summary>Add an image to a business gallery. Owner or Admin only.</summary>
    [HttpPost("/api/v1/businesses/{businessId:int}/images")]
    [Authorize]
    [ProducesResponseType(typeof(BusinessImageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Add(int businessId, [FromBody] AddBusinessImageDto request, CancellationToken ct)
    {
        var result = await imageService.AddAsync(businessId, request, ct);
        if (!result.IsSuccess) return MapResult(result);
        return Created($"/api/v1/businesses/{businessId}/images/{result.Value!.Id}", result.Value);
    }

    /// <summary>Remove an image from a business gallery. Owner or Admin only.</summary>
    [HttpDelete("/api/v1/businesses/{businessId:int}/images/{imageId:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Remove(int businessId, int imageId, CancellationToken ct)
        => MapResult(await imageService.RemoveAsync(businessId, imageId, ct));

    /// <summary>Set an image as the gallery cover. Owner or Admin only.</summary>
    [HttpPatch("/api/v1/businesses/{businessId:int}/images/{imageId:int}/cover")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SetCover(int businessId, int imageId, CancellationToken ct)
        => MapResult(await imageService.SetCoverAsync(businessId, imageId, ct));
}
