using Microsoft.Extensions.DependencyInjection;
using XpertStore.Application.Extensions.IdentityUser;
using XpertStore.Application.Models.Base;

namespace XpertStore.Infra.IoC;

public static class DependencyInjectionConfig
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {

        services.AddScoped<IAppIdentityUser, AppIdentityUser>();

        return services;
    }
}
