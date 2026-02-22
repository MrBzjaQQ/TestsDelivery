using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Infrastructure.Database.Context;
using StudentManagementService.Infrastructure.Database.Migrator;
using StudentManagementService.Infrastructure.Database.Repositories;

namespace StudentManagementService.Infrastructure.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StudentDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IStudentDbContext>(isp => isp.GetRequiredService<StudentDbContext>());
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ITestAssignmentRepository, TestAssignmentRepository>();
        services.AddScoped<ITestProgressRepository, TestProgressRepository>();
        services.AddScoped<IStudyGroupRepository, StudyGroupRepository>();
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
