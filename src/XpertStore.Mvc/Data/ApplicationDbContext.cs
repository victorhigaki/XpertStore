using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XpertStore.Entities.Models;

namespace XpertStore.Mvc.Data;

public class ApplicationDbContext : IdentityDbContext
{
    protected readonly IConfiguration Configuration;

    public ApplicationDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
    }

    public DbSet<Categoria> Categoria { get; set; } = default!;
    public DbSet<Produto> Produto { get; set; } = default!;
}
