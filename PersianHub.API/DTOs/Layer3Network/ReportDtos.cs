using PersianHub.API.Enums.Common;
using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.DTOs.Layer3Network;

public record CreateReportDto(
    int? AppUserId,
    ReferenceType ReferenceType,
    int ReferenceId,
    string Reason,
    string? Details
);

public record ReportCreatedDto(
    int Id,
    ReportStatus Status,
    DateTime CreatedAtUtc
);
