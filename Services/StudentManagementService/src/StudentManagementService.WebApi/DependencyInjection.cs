using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
using StudentManagementService.Application;
using StudentManagementService.Infrastructure.Database;
using StudentManagementService.WebApi.Factories;
using StudentManagementService.WebApi.Factories.Contract;
using StudentManagementService.WebApi.Handler;
using StudentManagementService.WebApi.HealthChecks;
using StudentManagementService.WebApi.Settings;

namespace StudentManagementService.WebApi;

public static class DependencyInjection
{
    private const string HealthCheckName = "PostgreSqlHealthCheck";

    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        builder.Services.AddDatabase(settings.ConnectionString);
        builder.Services.AddApplicationServices();

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
                Title = "Student Management Service API",
                Description = "API for managing students, test assignments, and progress tracking"
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
