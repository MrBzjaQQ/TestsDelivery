using FileStorageService.Application.Infrastructure.Database.Contract;
using FileStorageService.Infrastructure.Database.Context;
using FileStorageService.Infrastructure.Database.Migrator;
using FileStorageService.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileStorageService.Infrastructure.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FileStorageDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IFileStorageDbContext>(isp => isp.GetRequiredService<FileStorageDbContext>());
        services.AddScoped<IFileRepository, FileRepository>();
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
