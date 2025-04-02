using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace XpertStore.Business.Services.Interfaces;
public interface IUserService
{
    Task<IdentityUser> GetUserAsync(ClaimsPrincipal principal);
}
