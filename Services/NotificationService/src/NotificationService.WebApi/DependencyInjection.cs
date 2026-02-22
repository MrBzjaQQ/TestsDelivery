using NotificationService.Application;
using NotificationService.Infrastructure;
using NotificationService.Infrastructure.Database;
using NotificationService.WebApi.Factories;
using NotificationService.WebApi.Factories.Contract;
using NotificationService.WebApi.Handler;
using NotificationService.WebApi.HealthChecks;
using NotificationService.WebApi.Settings;

namespace NotificationService.WebApi;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddDatabase(settings.ConnectionString);
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);

        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.AddSingleton<IActivityWrapperFactory, ActivityWrapperFactory>();
        builder.Services.AddSingleton<Factories.Contract.IProblemDetailsFactory, ProblemDetailsFactory>();

        builder.Services.AddHealthChecks()
            .AddCheck<PostgreSqlHealthCheck>("PostgreSqlHealthCheck");

        return builder;
    }
}
