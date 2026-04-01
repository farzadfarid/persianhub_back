using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PersianHub.API.Auth;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated()
        => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public int GetUserId()
    {
        // DefaultInboundClaimTypeMap maps "sub" → ClaimTypes.NameIdentifier on read
        var value = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return int.TryParse(value, out var id) ? id : 0;
    }

    public string GetEmail()
        => _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Email) ?? string.Empty;

    public string GetRole()
        => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
}
