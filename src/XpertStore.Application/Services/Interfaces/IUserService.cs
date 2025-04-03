using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace XpertStore.Application.Services.Interfaces;
public interface IUserService
{
    Task<IdentityUser> GetUserAsync(ClaimsPrincipal principal);
    Task<IdentityUser?> GetUserByEmailAsync(string userEmail);
}
