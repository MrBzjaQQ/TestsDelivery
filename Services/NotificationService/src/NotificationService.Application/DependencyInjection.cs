using NotificationService.Application.Contracts;
using NotificationService.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NotificationService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailNotificationService, EmailNotificationService>();
        services.AddScoped<INotificationQueueService, NotificationQueueService>();

        services.AddSingleton<ITemplateService>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILogger<TemplateService>>();

            var basePath = configuration["EmailTemplates:BasePath"] ?? "Templates";
            var defaultLanguage = configuration["EmailTemplates:DefaultLanguage"] ?? "en";

            return new TemplateService(basePath, defaultLanguage, logger);
        });

        return services;
    }
}
