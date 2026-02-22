using Microsoft.EntityFrameworkCore;
using TestCheckingService.Domain.Entities;

namespace TestCheckingService.Infrastructure.Database.Context;

public interface ITestCheckingDbContext : IDisposable
{
    DbSet<Test> Tests { get; set; }

    DbSet<TestResult> TestResults { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
