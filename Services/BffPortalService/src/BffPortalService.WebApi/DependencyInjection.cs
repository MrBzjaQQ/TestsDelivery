using System.Reflection;
using BffPortalService.Application;
using BffPortalService.Infrastructure.Database;
using BffPortalService.Infrastructure.HttpClients;
using BffPortalService.Infrastructure.HttpClients.External.HttpClientFactories;
using BffPortalService.WebApi.Factories;
using BffPortalService.WebApi.Factories.Contract;
using BffPortalService.WebApi.Handler;
using BffPortalService.WebApi.HealthChecks;
using BffPortalService.WebApi.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi;

namespace BffPortalService.WebApi;

public static class DependencyInjection
{
    private const string HealthCheckName = "PostgreSqlHealthCheck";
    private const string ExternalHealthCheckName = "ExternalServicesHealthCheck";

    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddControllers();

        builder.Services.AddOpenApi();

        builder.Services.AddDatabase(settings.ConnectionString);
        builder.Services.AddApplicationServices();
        builder.Services.AddHttpClients(settings.ServiceUrls);

        builder.Services.AddMemoryCache();

        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.AddSingleton<IProblemDetailsFactory, ProblemDetailsFactory>();
        builder.Services.AddSingleton<IActivityWrapperFactory, ActivityWrapperFactory>();

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings.Jwt.Issuer,
                    ValidAudience = settings.Jwt.Audience,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(settings.Jwt.SecretKey))
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddHealthChecks()
            .AddCheck<PostgreSqlHealthCheck>(HealthCheckName, tags: ["database", "postgres"])
            .AddCheck<ExternalServiceHealthCheck>(ExternalHealthCheckName, tags: ["external"]);

        string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        builder.Services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(xmlPath);

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "BFF Portal Service API",
                Description = "Backend-For-Frontend API for TestsDelivery Portal"
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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            StatusCodeSelector = ex => ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                InvalidOperationException => StatusCodes.Status403Forbidden,
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
