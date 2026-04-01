using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Layer2Core;

/// <summary>
/// Tracks contact/lead events from users toward businesses.
/// Supports ContactType (Call, Message, WhatsApp, FormSubmit, ClickToContact) and conversion tracking.
/// </summary>
[Route("api/v1/contact-requests")]
public sealed class ContactRequestsController(IContactRequestService contactRequestService) : ApiControllerBase
{
    /// <summary>Creates a new contact request / lead record for a business.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ContactRequestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateContactRequestDto request, CancellationToken ct)
    {
        var result = await contactRequestService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns a contact request by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ContactRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await contactRequestService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns all contact requests received by a specific business.</summary>
    [HttpGet("business/{businessId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<ContactRequestListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByBusiness(int businessId, CancellationToken ct)
    {
        var result = await contactRequestService.GetByBusinessIdAsync(businessId, ct);
        return MapResult(result);
    }

    /// <summary>Returns all contact requests submitted by a specific user.</summary>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<ContactRequestListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUser(int userId, CancellationToken ct)
    {
        var result = await contactRequestService.GetByUserIdAsync(userId, ct);
        return MapResult(result);
    }

    /// <summary>Marks a contact request as converted (e.g., lead became a paying customer).</summary>
    [HttpPatch("{id:int}/convert")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkConverted(int id, CancellationToken ct)
    {
        var result = await contactRequestService.MarkConvertedAsync(id, ct);
        return MapResult(result);
    }
}
