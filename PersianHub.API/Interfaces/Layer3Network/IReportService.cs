using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer3Network;

namespace PersianHub.API.Interfaces.Layer3Network;

public interface IReportService
{
    Task<Result<ReportCreatedDto>> CreateAsync(CreateReportDto request, CancellationToken ct = default);
}
