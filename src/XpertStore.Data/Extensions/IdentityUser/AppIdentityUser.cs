﻿using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using XpertStore.Data.Models.Base;

namespace XpertStore.Data.Extensions.IdentityUser;

public class AppIdentityUser : IAppIdentityUser
{
    private readonly IHttpContextAccessor _accessor;

    public AppIdentityUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public bool IsAuthenticated()
    {
        return _accessor.HttpContext?.User.Identity is { IsAuthenticated: true };
    }

    public Guid GetUserId()
    {
        if (!IsAuthenticated()) return Guid.Empty;

        var claim = _accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claim))
            claim = _accessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        return claim is null ? Guid.Empty : Guid.Parse(claim);
    }

    public string GetUserName()
    {
        var username = _accessor.HttpContext?.User.FindFirst("username")?.Value;
        if (!string.IsNullOrEmpty(username)) return username;

        username = _accessor.HttpContext?.User.Identity?.Name;
        if (!string.IsNullOrEmpty(username)) return username;

        //username = _accessor.HttpContext?.User.FindFirst(JwtClaimTypes.Name)?.Value;
        //if (!string.IsNullOrEmpty(username)) return username;

        //username = _accessor.HttpContext?.User.FindFirst(JwtClaimTypes.GivenName)?.Value;
        //if (!string.IsNullOrEmpty(username)) return username;

        //var sub = _accessor.HttpContext?.User.FindFirst(JwtClaimTypes.Subject);
        //if (sub != null) return sub;

        return string.Empty;
    }

    public bool IsInRole(string role)
    {
        return _accessor.HttpContext != null && _accessor.HttpContext.User.IsInRole(role);
    }

    public string GetLocalIpAddress()
    {
        return _accessor.HttpContext?.Connection.LocalIpAddress?.ToString();

    }

    public string GetRemoteIdAddress()
    {
        return _accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    }
}
