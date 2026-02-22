using BffPortalService.Application.Contracts;
using BffPortalService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BffPortalService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IStudentPortalService, StudentPortalService>();
        services.AddScoped<ITestsPortalService, TestsPortalService>();
        services.AddScoped<IGroupAnalyticsService, GroupAnalyticsService>();
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}
