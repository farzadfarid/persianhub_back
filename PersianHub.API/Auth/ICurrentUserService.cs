namespace PersianHub.API.Auth;

public interface ICurrentUserService
{
    int GetUserId();
    string GetEmail();
    string GetRole();
    bool IsAuthenticated();
}
