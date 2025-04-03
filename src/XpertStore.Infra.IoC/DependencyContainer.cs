using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using XpertStore.Application.Services;
using XpertStore.Application.Services.Interfaces;

namespace XpertStore.Infra.IoC;

public static class DependencyContainer
{
    public static IServiceCollection RegisterServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
