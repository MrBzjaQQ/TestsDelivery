using BffPortalService.Infrastructure.Database.Context;
using BffPortalService.Infrastructure.Database.Migrator;
using BffPortalService.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BffPortalService.Infrastructure.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<BffPortalDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IBffPortalDbContext>(isp => isp.GetRequiredService<BffPortalDbContext>());
        services.AddScoped<PortalCacheRepository>();
        services.AddScoped<UnitOfWork>();
        services.AddTransient<DatabaseMigrator>();

        return services;
    }

    public static IHost MigrateDatabase(this IHost app)
    {
        using var scope = app.Services.CreateAsyncScope();
        var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
        migrator.Migrate();
        return app;
    }
}
