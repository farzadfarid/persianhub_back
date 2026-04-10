using PersianHub.API.Enums.Common;
using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.DTOs.Admin;

public record AdminReportListItemDto(
    int Id,
    int? AppUserId,
    string? UserEmail,
    ReferenceType ReferenceType,
    int ReferenceId,
    string Reason,
    ReportStatus Status,
    DateTime CreatedAtUtc
);

public record AdminReportDetailDto(
    int Id,
    int? AppUserId,
    string? UserEmail,
    ReferenceType ReferenceType,
    int ReferenceId,
    string Reason,
    string? Details,
    ReportStatus Status,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);
