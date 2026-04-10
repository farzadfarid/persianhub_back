using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Controllers.Layer3Network;

/// <summary>
/// Allows users to submit abuse/content reports.
/// </summary>
[Route("api/v1/reports")]
public sealed class ReportsController(IReportService reportService) : ApiControllerBase
{
    /// <summary>Submits a new content report.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReportCreatedDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateReportDto request, CancellationToken ct)
    {
        var result = await reportService.CreateAsync(request, ct);
        return MapCreated(result, null, null);
    }
}
