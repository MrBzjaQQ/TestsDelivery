using QuestionManagementService.WebApi;
using QuestionManagementService.WebApi.Settings;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var settings = builder.Configuration.Get<AppSettings>();

builder.ConfigureBuilder(settings);

var app = builder.Build();

app.MapDefaultEndpoints();

app.ConfigureApplication().Run();
