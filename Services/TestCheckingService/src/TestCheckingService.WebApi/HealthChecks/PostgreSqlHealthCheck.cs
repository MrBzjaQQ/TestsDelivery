using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Npgsql;
using TestCheckingService.WebApi.Settings;

namespace TestCheckingService.WebApi.HealthChecks;

public class PostgreSqlHealthCheck : IHealthCheck
{
    private const string HealthPostgreSqlQuery = "SELECT 1";
    private readonly ILogger<PostgreSqlHealthCheck> _logger;
    private readonly string _connectionString;

    public PostgreSqlHealthCheck(ILogger<PostgreSqlHealthCheck> logger, IOptions<AppSettings> appSettings)
    {
        _logger = logger;
        _connectionString = appSettings.Value.ConnectionString;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new NpgsqlCommand(HealthPostgreSqlQuery, connection);
            object? result = await command.ExecuteScalarAsync(cancellationToken);

            var healthCheckName = $"{context.Registration.Name}_result";
            var healthCheckData = new Dictionary<string, object> { [healthCheckName] = result! };

            return result != null
                ? HealthCheckResult.Healthy("PostgreSQL DB Query Succeeded", healthCheckData)
                : HealthCheckResult.Unhealthy("PostgreSQL DB Query Failed", data: healthCheckData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PostgreSQL health check failed");
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }
}
