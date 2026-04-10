using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin management of deal categories.
/// </summary>
[Route("api/v1/admin/deal-categories")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminDealCategoriesController(IDealCategoryService categoryService) : ApiControllerBase
{
    /// <summary>Returns all deal categories including inactive ones.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DealCategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => MapResult(await categoryService.GetAllAsync(ct));

    /// <summary>Returns a deal category by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DealCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await categoryService.GetByIdAsync(id, ct));

    /// <summary>Creates a new deal category.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(DealCategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] UpsertDealCategoryDto dto, CancellationToken ct)
    {
        var result = await categoryService.CreateAsync(dto, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Updates an existing deal category.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(DealCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpsertDealCategoryDto dto, CancellationToken ct)
        => MapResult(await categoryService.UpdateAsync(id, dto, ct));

    /// <summary>Activates a deal category.</summary>
    [HttpPut("{id:int}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
        => MapResult(await categoryService.SetActiveStatusAsync(id, true, ct));

    /// <summary>Deactivates a deal category.</summary>
    [HttpPut("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
        => MapResult(await categoryService.SetActiveStatusAsync(id, false, ct));

    /// <summary>Deletes a deal category.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => MapResult(await categoryService.DeleteAsync(id, ct));
}
