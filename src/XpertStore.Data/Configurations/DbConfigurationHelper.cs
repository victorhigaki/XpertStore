using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XpertStore.Data.Data;
using XpertStore.Entities.Models;

namespace XpertStore.Data.Configurations
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {

        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var contextId = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (env.IsDevelopment())
            {
                await contextId.Database.MigrateAsync();
            }

            await EnsureSeedProducts(contextId);
        }

        public static async Task EnsureSeedProducts(ApplicationDbContext contextId)
        {
            if (contextId.Users.Any())
                return;
            var userId = Guid.NewGuid();
            await contextId.Users.AddAsync(new IdentityUser
            {
                Id = userId.ToString(),
                UserName = "teste@teste.com",
                NormalizedUserName = "TESTE@TESTE.COM",
                Email = "teste@teste.com",
                NormalizedEmail = "TESTE@TESTE.COM",
                AccessFailedCount = 0,
                LockoutEnabled = false,
                PasswordHash = "AQAAAAIAAYagAAAAEEdWhqiCwW/jZz0hEM7aNjok7IxniahnxKxxO5zsx2TvWs4ht1FUDnYofR8JKsA5UA==",
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            });

            await contextId.AddAsync(new Vendedor
            {
                Id = userId
            });

            var categoriaId = Guid.NewGuid();

            await contextId.AddRangeAsync([
                new Categoria {
                    Id = categoriaId,
                    Nome = "Categoria 1",
                    Descricao = "Descricao Categoria 1"
                },
                new Categoria {
                    Id = Guid.NewGuid(),
                    Nome = "Categoria 2",
                    Descricao = "Descricao Categoria 2"
                }
            ]);

            await contextId.AddAsync(new Produto
            {
                Id = Guid.NewGuid(),
                Nome = "Nome Teste",
                Descricao = "Descricao Teste",
                Imagem = "",
                Preco = 456,
                Estoque = 123,
                CategoriaId = categoriaId,
                VendedorId = userId,
            });

            await contextId.SaveChangesAsync();
        }
    }
}
