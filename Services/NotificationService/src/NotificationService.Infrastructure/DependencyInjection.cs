using NotificationService.Application.Contracts;
using NotificationService.Infrastructure.Email;
using NotificationService.Infrastructure.MassTransit.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;

namespace NotificationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpSettings = new SmtpSettings
        {
            Host = configuration["Smtp:Host"] ?? "localhost",
            Port = int.Parse(configuration["Smtp:Port"] ?? "587"),
            Username = configuration["Smtp:Username"] ?? string.Empty,
            Password = configuration["Smtp:Password"] ?? string.Empty,
            From = configuration["Smtp:From"] ?? "noreply@testsdelivery.com",
            DisplayName = configuration["Smtp:DisplayName"] ?? "TestsDelivery",
            EnableSsl = bool.Parse(configuration["Smtp:EnableSsl"] ?? "true")
        };

        services.AddSingleton(smtpSettings);
        services.AddScoped<IEmailSender, EmailSender>();

        return services;
    }

    public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<TestCreatedConsumer>()
                .Endpoint(e => e.Name = "notification-test-created");

            x.AddConsumer<TestResultConsumer>()
                .Endpoint(e => e.Name = "notification-test-result");

            x.AddConsumer<UserRegisteredConsumer>()
                .Endpoint(e => e.Name = "notification-user-registered");

            x.UsingRabbitMq((context, cfg) =>
            {
                var host = configuration["RabbitMQ:Host"] ?? "localhost";
                var port = ushort.Parse(configuration["RabbitMQ:Port"] ?? "5672");
                var username = configuration["RabbitMQ:Username"] ?? "guest";
                var password = configuration["RabbitMQ:Password"] ?? "guest";
                var virtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/";

                cfg.Host(host, port, virtualHost, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.UseMessageRetry(retry => retry.Intervals(
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5)));

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
