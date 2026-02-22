using QuestionManagementService.Application.Infrastructure.Database.Contract;
using QuestionManagementService.Infrastructure.Database.Context;
using QuestionManagementService.Infrastructure.Database.Migrator;
using QuestionManagementService.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace QuestionManagementService.Infrastructure.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<QuestionDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IQuestionDbContext>(isp => isp.GetRequiredService<QuestionDbContext>());
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IQuestionBankRepository, QuestionBankRepository>();
        services.AddScoped<ITestRepository, TestRepository>();
        services.AddScoped<ITestTemplateRepository, TestTemplateRepository>();
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
