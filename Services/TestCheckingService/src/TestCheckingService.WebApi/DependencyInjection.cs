using System.Reflection;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
using TestCheckingService.Application;
using TestCheckingService.Application.MassTransit.Consumers;
using TestCheckingService.Application.Settings;
using TestCheckingService.Infrastructure.Database;
using TestCheckingService.WebApi.Factories;
using TestCheckingService.WebApi.Factories.Contract;
using TestCheckingService.WebApi.Handler;
using TestCheckingService.WebApi.HealthChecks;
using AppSettings = TestCheckingService.WebApi.Settings.AppSettings;

namespace TestCheckingService.WebApi;

public static class DependencyInjection
{
    private const string HealthCheckName = "PostgreSqlHealthCheck";

    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        builder.Services.AddDatabase(settings.ConnectionString);
        builder.Services.AddApplicationServices();

        builder.Services.AddSingleton(new ScoringSettings
        {
            PassPercentage = settings.Scoring.PassPercentage,
            AllowRetries = settings.Scoring.AllowRetries,
            MaxAttempts = settings.Scoring.MaxAttempts,
            AttemptCooldownMinutes = settings.Scoring.AttemptCooldownMinutes
        });

        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumer<TestSubmittedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri($"rabbitmq://{settings.RabbitMQ.Host}:{settings.RabbitMQ.Port}"), h =>
                {
                    h.Username(settings.RabbitMQ.Username);
                    h.Password(settings.RabbitMQ.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.AddSingleton<IProblemDetailsFactory, ProblemDetailsFactory>();
        builder.Services.AddSingleton<IActivityWrapperFactory, ActivityWrapperFactory>();

        builder.Services.AddHealthChecks()
            .AddCheck<PostgreSqlHealthCheck>(HealthCheckName, tags: ["database", "postgres"]);

        string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        builder.Services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(xmlPath);

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Test Checking Service API",
                Description = "API for checking tests and managing test results"
            });
        });

        return builder;
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            StatusCodeSelector = ex => ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            }
        });

        app.MigrateDatabase();

        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHealthChecks("/quickhealth", new HealthCheckOptions
        {
            Predicate = _ => false
        });

        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}
