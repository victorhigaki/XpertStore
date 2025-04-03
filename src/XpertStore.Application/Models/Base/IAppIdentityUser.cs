namespace XpertStore.Application.Models.Base;

public interface IAppIdentityUser
{
    string GetUserName();
    Guid GetUserId();
    bool IsAuthenticated();
    bool IsInRole(string role);
    string GetRemoteIdAddress();
    string GetLocalIpAddress();
}
