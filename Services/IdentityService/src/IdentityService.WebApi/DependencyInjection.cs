using System.Text;
using IdentityService.Application.Settings;
using IdentityService.Infrastructure.Database;
using IdentityService.WebApi.Factories;
using IdentityService.WebApi.Handler;
using IdentityService.WebApi.HealthChecks;
using IdentityService.WebApi.Settings;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace IdentityService.WebApi;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddDatabase(settings.ConnectionString);
        builder.Services.AddIdentityServices(builder.Configuration);

        builder.Services.AddJwtAuthentication(builder.Configuration);

        builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);

        builder.Services.AddSingleton<IProblemDetailsFactory, ProblemDetailsFactory>();
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.AddHealthChecks()
            .AddCheck<PostgreSqlHealthCheck>("PostgreSqlHealthCheck");

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "File Storage Service API",
                Description = "API for uploading, downloading, and managing files",
            });
        });

        return builder;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings?.Issuer,
                ValidAudience = jwtSettings?.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.SecretKey ?? string.Empty)),
                ClockSkew = TimeSpan.Zero,
            };
        });

        return services;
    }

    private static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMQ").Get<RabbitMqSettings>();

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                var host = rabbitMqSettings?.Host ?? "localhost";
                var port = rabbitMqSettings?.Port ?? 5672;
                var virtualHost = rabbitMqSettings?.VirtualHost ?? "/";
                var username = rabbitMqSettings?.Username ?? "guest";
                var password = rabbitMqSettings?.Password ?? "guest";

                cfg.Host(new Uri($"rabbitmq://{host}:{port}/{virtualHost.TrimStart('/')}"), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
