using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TestCheckingService.Infrastructure.Database.Repositories;
using TestCheckingService.Tests.Integration.TestInfrastructure;
using Xunit;

namespace TestCheckingService.Tests.Integration.Controllers;

[Collection("Database")]
public class TestCheckControllerTests : DbTestsBase
{
    private readonly TestResultRepository _testResultRepository;
    private readonly TestRepository _testRepository;

    public TestCheckControllerTests(DatabaseFixture fixture) : base(fixture)
    {
        _testResultRepository = new TestResultRepository(DbContext);
        _testRepository = new TestRepository(DbContext);
    }

    [Fact]
    public async Task Database_Should_BeConnected()
    {
        var canConnect = await DbContext.Database.CanConnectAsync();
        canConnect.Should().BeTrue();
    }

    [Fact]
    public async Task TestRepository_Should_StoreAndRetrieveTest()
    {
        var testId = Guid.NewGuid();
        var test = new Domain.Entities.Test
        {
            Id = testId,
            Title = "Integration Test",
            PassPercentage = 70,
            MaxScore = 100,
            CreatedAt = DateTime.UtcNow
        };

        await _testRepository.AddAsync(test, CancellationToken.None);

        var retrieved = await _testRepository.GetByIdAsync(testId, CancellationToken.None);

        retrieved.Should().NotBeNull();
        retrieved!.Title.Should().Be("Integration Test");
        retrieved.PassPercentage.Should().Be(70);
        retrieved.MaxScore.Should().Be(100);
    }

    [Fact]
    public async Task TestResultRepository_Should_StoreAndRetrieveResult()
    {
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();

        var test = new Domain.Entities.Test
        {
            Id = testId,
            Title = "Test for Result",
            PassPercentage = 70,
            MaxScore = 100,
            CreatedAt = DateTime.UtcNow
        };

        await _testRepository.AddAsync(test, CancellationToken.None);

        var testResult = new Domain.Entities.TestResult
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

        await _testResultRepository.AddAsync(testResult, CancellationToken.None);

        var retrieved = await _testResultRepository.GetByIdAsync(testId, studentId, CancellationToken.None);

        retrieved.Should().NotBeNull();
        retrieved!.Score.Should().Be(85);
        retrieved.IsPassed.Should().BeTrue();
        retrieved.Percentage.Should().Be(85);
    }

    [Fact]
    public async Task TestResultRepository_Should_GetResultsByTestId()
    {
        var testId = Guid.NewGuid();

        var test = new Domain.Entities.Test
        {
            Id = testId,
            Title = "Test",
            PassPercentage = 70,
            MaxScore = 100,
            CreatedAt = DateTime.UtcNow
        };

        await _testRepository.AddAsync(test, CancellationToken.None);

        for (int i = 0; i < 3; i++)
        {
            var testResult = new Domain.Entities.TestResult
            {
                Id = Guid.NewGuid(),
                TestId = testId,
                StudentId = Guid.NewGuid(),
                Score = 80,
                MaxScore = 100,
                Percentage = 80,
                IsPassed = true,
                AttemptNumber = 1,
                Answers = "[]",
                CreatedAt = DateTime.UtcNow
            };

            await _testResultRepository.AddAsync(testResult, CancellationToken.None);
        }

        var results = await _testResultRepository.GetByTestIdAsync(testId, CancellationToken.None);

        results.Should().HaveCount(3);
    }

    [Fact]
    public async Task TestResultRepository_Should_GetAttemptCount()
    {
        var testId = Guid.NewGuid();
        var studentId = Guid.NewGuid();

        var test = new Domain.Entities.Test
        {
            Id = testId,
            Title = "Test",
            PassPercentage = 70,
            MaxScore = 100,
            CreatedAt = DateTime.UtcNow
        };

        await _testRepository.AddAsync(test, CancellationToken.None);

        for (int i = 0; i < 2; i++)
        {
            var testResult = new Domain.Entities.TestResult
            {
                Id = Guid.NewGuid(),
                TestId = testId,
                StudentId = studentId,
                Score = 80,
                MaxScore = 100,
                Percentage = 80,
                IsPassed = true,
                AttemptNumber = i + 1,
                Answers = "[]",
                CreatedAt = DateTime.UtcNow
            };

            await _testResultRepository.AddAsync(testResult, CancellationToken.None);
        }

        var count = await _testResultRepository.GetAttemptCountAsync(testId, studentId, CancellationToken.None);

        count.Should().Be(2);
    }
}
