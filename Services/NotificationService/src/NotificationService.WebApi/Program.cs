using NotificationService.Infrastructure.Database;
using NotificationService.WebApi;
using NotificationService.WebApi.Settings;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var appSettings = new AppSettings
{
    ConnectionString = builder.Configuration["ConnectionString"] ?? string.Empty,
    Smtp = new SmtpSettings
    {
        Host = builder.Configuration["Smtp:Host"] ?? "localhost",
        Port = int.Parse(builder.Configuration["Smtp:Port"] ?? "1025"),
        Username = builder.Configuration["Smtp:Username"] ?? string.Empty,
        Password = builder.Configuration["Smtp:Password"] ?? string.Empty,
        From = builder.Configuration["Smtp:From"] ?? "noreply@testsdelivery.com",
        DisplayName = builder.Configuration["Smtp:DisplayName"] ?? "TestsDelivery",
        EnableSsl = bool.Parse(builder.Configuration["Smtp:EnableSsl"] ?? "false")
    }
};

builder.ConfigureBuilder(appSettings);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "NotificationService API v1");
    });
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapDefaultEndpoints();

app.MigrateDatabase();

app.Run();
