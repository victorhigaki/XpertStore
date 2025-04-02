using XpertStore.Business.Services;
using XpertStore.Business.Services.Interfaces;
using XpertStore.Data.Configurations;
using XpertStore.Mvc.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabaseSelector();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseDbMigrationHelper();

app.Run();
