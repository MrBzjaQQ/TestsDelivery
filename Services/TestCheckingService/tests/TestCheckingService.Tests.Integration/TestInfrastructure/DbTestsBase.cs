using Microsoft.EntityFrameworkCore;
using TestCheckingService.Domain.Entities;
using TestCheckingService.Infrastructure.Database.Context;
using Xunit;

namespace TestCheckingService.Tests.Integration.TestInfrastructure;

[Collection("Database")]
public class DbTestsBase : IAsyncLifetime
{
    protected readonly DatabaseFixture Fixture;
    protected readonly TestCheckingDbContext DbContext;

    public DbTestsBase(DatabaseFixture fixture)
    {
        Fixture = fixture;

        var options = new DbContextOptionsBuilder<TestCheckingDbContext>()
            .UseNpgsql(Fixture.DbContainer.GetConnectionString())
            .Options;

        DbContext = new TestCheckingDbContext(options);
    }

    public async Task InitializeAsync()
    {
        await DbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
    }

    protected async Task SeedTestDataAsync()
    {
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();

        var test = new Test
        {
            Id = testId,
            Title = "Integration Test",
            PassPercentage = 70,
            MaxScore = 100,
            CreatedAt = DateTime.UtcNow
        };

        await DbContext.Tests.AddAsync(test);

        var testResult = new TestResult
        {
            Id = Guid.NewGuid(),
            TestId = testId,
            StudentId = studentId,
            Score = 85,
            MaxScore = 100,
            Percentage = 85,
            IsPassed = true,
            AttemptNumber = 1,
            PassedDate = DateTime.UtcNow,
            Answers = "[]",
            CreatedAt = DateTime.UtcNow
        };

        await DbContext.TestResults.AddAsync(testResult);
        await DbContext.SaveChangesAsync();
    }
}
