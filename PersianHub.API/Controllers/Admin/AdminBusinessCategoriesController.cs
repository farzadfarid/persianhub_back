using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin management of business categories.
/// </summary>
[Route("api/v1/admin/business-categories")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminBusinessCategoriesController(IBusinessCategoryService categoryService) : ApiControllerBase
{
    /// <summary>Returns all business categories including inactive ones.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BusinessCategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => MapResult(await categoryService.GetAllAsync(ct));

    /// <summary>Returns a business category by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BusinessCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await categoryService.GetByIdAsync(id, ct));

    /// <summary>Creates a new business category.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BusinessCategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] UpsertBusinessCategoryDto dto, CancellationToken ct)
    {
        var result = await categoryService.CreateAsync(dto, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Updates an existing business category.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BusinessCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpsertBusinessCategoryDto dto, CancellationToken ct)
        => MapResult(await categoryService.UpdateAsync(id, dto, ct));

    /// <summary>Activates a business category — makes it visible to users.</summary>
    [HttpPut("{id:int}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
        => MapResult(await categoryService.SetActiveStatusAsync(id, true, ct));

    /// <summary>Deactivates a business category — hides it from users.</summary>
    [HttpPut("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
        => MapResult(await categoryService.SetActiveStatusAsync(id, false, ct));

    /// <summary>Deletes a business category. Fails if businesses are linked.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => MapResult(await categoryService.DeleteAsync(id, ct));
}
