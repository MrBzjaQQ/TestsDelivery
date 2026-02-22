using IdentityService.Infrastructure.Database;
using IdentityService.WebApi;
using IdentityService.WebApi.Settings;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var settings = new AppSettings
{
    ConnectionString = builder.Configuration["ConnectionString"] ?? string.Empty,
    AppName = builder.Configuration["AppName"] ?? "identity-service:test",
};

builder.ConfigureBuilder(settings);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapDefaultEndpoints();
app.UseExceptionHandler();

app.MigrateDatabase();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

public partial class Program
{
}
