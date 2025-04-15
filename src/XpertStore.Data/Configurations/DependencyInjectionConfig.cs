using Microsoft.Extensions.DependencyInjection;
using XpertStore.Data.Extensions.IdentityUser;
using XpertStore.Data.Models.Base;
using XpertStore.Data.Repositories;
using XpertStore.Data.Repositories.Interfaces;

namespace XpertStore.Data.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAppIdentityUser, AppIdentityUser>();

        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IVendedorRepository, VendedorRepository>();

        return services;
    }
}
