using PersianHub.API.Common;
using PersianHub.API.Enums.Common;
using PersianHub.API.Entities.Layer2Core;

namespace PersianHub.API.Entities.Common;

public class AppUser : AuditableEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? FirstNameFa { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? LastNameFa { get; set; }
    public string? DisplayName { get; set; }
    public string? DisplayNameFa { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? Bio { get; set; }
    public string? BioFa { get; set; }
    public PreferredLanguage PreferredLanguage { get; set; } = PreferredLanguage.Persian;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = AppRoles.User;
    public bool IsActive { get; set; }

    // Navigation
    public ICollection<Business> Businesses { get; set; } = [];
}
