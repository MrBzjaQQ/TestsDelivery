using IdentityService.Application.Contracts;
using IdentityService.Infrastructure.Database.Context;
using IdentityService.Infrastructure.Database.Identity;
using IdentityService.Infrastructure.Database.Migrator;
using IdentityService.Infrastructure.Database.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityService.Infrastructure.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IIdentityDbContext>(isp => isp.GetRequiredService<IdentityDbContext>());

        services.AddTransient<DatabaseMigrator>();

        return services;
    }

    public static IdentityBuilder AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        return services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequiredLength = configuration.GetValue("Identity:PasswordRequiredLength", 8);
            options.Password.RequireDigit = configuration.GetValue("Identity:PasswordRequireDigit", true);
            options.Password.RequireLowercase = configuration.GetValue("Identity:PasswordRequireLowercase", true);
            options.Password.RequireUppercase = configuration.GetValue("Identity:PasswordRequireUppercase", true);
            options.Password.RequireNonAlphanumeric = configuration.GetValue("Identity:PasswordRequireNonAlphanumeric", true);
            options.User.RequireUniqueEmail = configuration.GetValue("Identity:UserRequireUniqueEmail", true);
            options.SignIn.RequireConfirmedEmail = configuration.GetValue("Identity:EmailConfirmationRequired", true);
        })
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders();
    }

    public static IHost MigrateDatabase(this IHost app)
    {
        using var scope = app.Services.CreateAsyncScope();
        var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
        migrator.Migrate();
        return app;
    }
}
