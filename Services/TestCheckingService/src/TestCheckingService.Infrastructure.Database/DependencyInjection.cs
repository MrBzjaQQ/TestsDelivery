using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestCheckingService.Application.Infrastructure.Database.Contract;
using TestCheckingService.Infrastructure.Database.Context;
using TestCheckingService.Infrastructure.Database.Migrator;
using TestCheckingService.Infrastructure.Database.Repositories;

namespace TestCheckingService.Infrastructure.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TestCheckingDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ITestCheckingDbContext>(isp => isp.GetRequiredService<TestCheckingDbContext>());
        services.AddScoped<ITestRepository, TestRepository>();
        services.AddScoped<ITestResultRepository, TestResultRepository>();
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
