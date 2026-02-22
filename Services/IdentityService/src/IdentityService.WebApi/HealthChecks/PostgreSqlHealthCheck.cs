using IdentityService.Infrastructure.Database.Context;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IdentityService.WebApi.HealthChecks;

public class PostgreSqlHealthCheck : IHealthCheck
{
    private readonly IdentityDbContext _dbContext;
    private readonly ILogger<PostgreSqlHealthCheck> _logger;

    public PostgreSqlHealthCheck(
        IdentityDbContext dbContext,
        ILogger<PostgreSqlHealthCheck> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);

            if (canConnect)
            {
                return HealthCheckResult.Healthy("PostgreSQL connection is healthy");
            }

            return HealthCheckResult.Unhealthy("Cannot connect to PostgreSQL");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PostgreSQL health check failed");
            return HealthCheckResult.Unhealthy("PostgreSQL health check failed", ex);
        }
    }
}
