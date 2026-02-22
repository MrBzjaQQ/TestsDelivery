using System.Reflection;
using QuestionManagementService.Application;
using QuestionManagementService.Infrastructure.Database;
using QuestionManagementService.WebApi.Factories;
using QuestionManagementService.WebApi.Factories.Contract;
using QuestionManagementService.WebApi.Handler;
using QuestionManagementService.WebApi.HealthChecks;
using QuestionManagementService.WebApi.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi;

namespace QuestionManagementService.WebApi;

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
                Title = "Question Management Service API",
                Description = "API for managing questions, question banks, tests and templates"
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
                QuestionManagementService.Domain.Exceptions.QuestionNotFoundException => StatusCodes.Status404NotFound,
                QuestionManagementService.Domain.Exceptions.QuestionBankNotFoundException => StatusCodes.Status404NotFound,
                QuestionManagementService.Domain.Exceptions.TestNotFoundException => StatusCodes.Status404NotFound,
                QuestionManagementService.Domain.Exceptions.TestTemplateNotFoundException => StatusCodes.Status404NotFound,
                QuestionManagementService.Domain.Exceptions.TestValidationException => StatusCodes.Status400BadRequest,
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
