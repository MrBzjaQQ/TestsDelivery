using NotificationService.Application.Infrastructure.Database.Contract;
using NotificationService.Infrastructure.Database.Context;
using NotificationService.Infrastructure.Database.Migrator;
using NotificationService.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NotificationService.Infrastructure.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<NotificationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<INotificationDbContext>(isp => isp.GetRequiredService<NotificationDbContext>());
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
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
