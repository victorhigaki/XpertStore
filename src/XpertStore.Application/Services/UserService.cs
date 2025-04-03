using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using XpertStore.Application.Services.Interfaces;

namespace XpertStore.Application.Services;
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

    public async Task<IdentityUser?> GetUserByEmailAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        return user;
    }
}
