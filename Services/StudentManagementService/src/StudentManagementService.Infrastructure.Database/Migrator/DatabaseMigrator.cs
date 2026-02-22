using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementService.Infrastructure.Database.Context;

namespace StudentManagementService.Infrastructure.Database.Migrator;

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
        var context = scope.ServiceProvider.GetRequiredService<StudentDbContext>();
        context.Database.Migrate();
    }
}
