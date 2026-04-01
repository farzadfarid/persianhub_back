using PersianHub.API.Common;
using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Entities.Common;

namespace PersianHub.API.Entities.Layer2Core;

public class ContactRequest : AuditableEntity
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public int? AppUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Message { get; set; }
    public ContactType ContactType { get; set; } = ContactType.FormSubmit;
    public ContactRequestStatus Status { get; set; } = ContactRequestStatus.New;
    public bool IsConverted { get; set; }
    public string? Metadata { get; set; }

    // Navigation
    public Business Business { get; set; } = null!;
    public AppUser? AppUser { get; set; }
}
