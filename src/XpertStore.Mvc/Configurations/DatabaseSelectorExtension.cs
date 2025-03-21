using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XpertStore.Mvc.Data;

namespace XpertStore.Mvc.Configurations
{
    public static class DatabaseSelectorExtension
    {
        public static void AddDatabaseSelector(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<ApplicationDbContext, SqliteDbContext>();
                builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<SqliteDbContext>();
            }
            else
            {
                builder.Services.AddDbContext<ApplicationDbContext>();
                builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            }
        }
    }
}
