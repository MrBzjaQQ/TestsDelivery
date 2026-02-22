using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestCheckingService.Infrastructure.Database.Context;

namespace TestCheckingService.Infrastructure.Database.Migrator;

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
        var context = scope.ServiceProvider.GetRequiredService<TestCheckingDbContext>();
        context.Database.Migrate();
    }
}
