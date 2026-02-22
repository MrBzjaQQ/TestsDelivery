using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestCheckingService.Application.Contracts;
using TestCheckingService.Application.Infrastructure.Database.Contract;
using TestCheckingService.Application.Scoring;
using TestCheckingService.Application.Settings;

namespace TestCheckingService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITestCheckService, TestCheckService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IScoringEngine, ScoringEngine>();

        services.AddOptions<ScoringSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("Scoring").Bind(settings);
            });

        return services;
    }
}
