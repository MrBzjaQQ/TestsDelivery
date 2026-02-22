using NotificationService.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NotificationService.Infrastructure.Database.Migrator;

public class DatabaseMigrator
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Migrate()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
        context.Database.Migrate();
    }
}
