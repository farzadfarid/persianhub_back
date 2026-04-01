using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;

namespace PersianHub.API.Interfaces.Layer2Core;

public interface IBusinessImageService
{
    Task<Result<IReadOnlyList<BusinessImageDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default);
    Task<Result<BusinessImageDto>> AddAsync(int businessId, AddBusinessImageDto request, CancellationToken ct = default);
    Task<Result> RemoveAsync(int businessId, int imageId, CancellationToken ct = default);
    Task<Result> SetCoverAsync(int businessId, int imageId, CancellationToken ct = default);
}
