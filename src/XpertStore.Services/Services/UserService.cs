using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using XpertStore.Business.Services.Interfaces;

namespace XpertStore.Business.Services;
public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityUser> GetUserAsync(ClaimsPrincipal principal)
    {
        return await _userManager.GetUserAsync(principal);
    }
}
