using XpertStore.Api.Configuration;
using XpertStore.Data.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabaseSelector();

builder.Services.AddControllers();

builder
    .AddCorsConfig()
    .AddSwaggerConfig();

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

app.UseAuthorization();

app.MapControllers();

app.UseDbMigrationHelper();

app.Run();
