using XpertStore.Api.Configuration;
using XpertStore.Application.Services;
using XpertStore.Application.Services.Interfaces;
using XpertStore.Data.Configurations;
using XpertStore.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddApiConfig()
    .AddCorsConfig()
    .AddSwaggerConfig()
    .AddDatabaseSelector()
    .AddIdentityConfig()
    .RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("Development");
}
else
{
    app.UseCors("Production");
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseDbMigrationHelper();

app.Run();
