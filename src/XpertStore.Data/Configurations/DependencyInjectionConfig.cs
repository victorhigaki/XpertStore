using Microsoft.Extensions.DependencyInjection;
using XpertStore.Data.Extensions.IdentityUser;
using XpertStore.Data.Models.Base;

namespace XpertStore.Data.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {

        services.AddScoped<IAppIdentityUser, AppIdentityUser>();

        return services;
    }
}
