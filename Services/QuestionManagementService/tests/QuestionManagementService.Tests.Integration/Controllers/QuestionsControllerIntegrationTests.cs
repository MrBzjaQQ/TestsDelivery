using Microsoft.EntityFrameworkCore;
using QuestionManagementService.Domain.Entities;
using QuestionManagementService.Infrastructure.Database.Context;
using QuestionManagementService.Tests.Integration.TestInfrastructure;
using Xunit;
using FluentAssertions;

namespace QuestionManagementService.Tests.Integration.Controllers;

[Collection("Database")]
public class QuestionsControllerIntegrationTests : IAsyncLifetime
{
    private readonly DatabaseFixture _fixture;
    private QuestionDbContext _dbContext = null!;

    public QuestionsControllerIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<QuestionDbContext>()
            .UseNpgsql(_fixture.DbContainer.GetConnectionString())
            .Options;

        _dbContext = new QuestionDbContext(options);
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }

    [Fact]
    public async Task FullWorkflow_Should_Work()
    {
        var bankId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();

        var bank = new QuestionBank
        {
            Id = bankId,
            Name = "Integration Test Bank",
            Description = "Bank for integration testing",
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.QuestionBanks.Add(bank);
        await _dbContext.SaveChangesAsync();

        var question = new Question
        {
            Id = Guid.NewGuid(),
            Text = "What is 2+2?",
            Category = "Math",
            Difficulty = 1,
            QuestionBankId = bankId,
            CreatedAt = DateTime.UtcNow,
            AnswerOptions =
            [
                new() { Id = Guid.NewGuid(), QuestionId = Guid.NewGuid(), Text = "3", IsCorrect = false, Ordinal = 0 },
                new() { Id = Guid.NewGuid(), QuestionId = Guid.NewGuid(), Text = "4", IsCorrect = true, Ordinal = 1 },
                new() { Id = Guid.NewGuid(), QuestionId = Guid.NewGuid(), Text = "5", IsCorrect = false, Ordinal = 2 }
            ]
        };

        question.AnswerOptions = question.AnswerOptions.Select(o => { o.QuestionId = question.Id; return o; }).ToList();
        _dbContext.Questions.Add(question);
        await _dbContext.SaveChangesAsync();

        var savedQuestion = await _dbContext.Questions
            .Include(q => q.AnswerOptions)
            .FirstOrDefaultAsync(q => q.Id == question.Id);

        savedQuestion.Should().NotBeNull();
        savedQuestion!.Text.Should().Be("What is 2+2?");
        savedQuestion.AnswerOptions.Should().HaveCount(3);
        savedQuestion.AnswerOptions.Count(o => o.IsCorrect).Should().Be(1);
    }

    [Fact]
    public async Task QuestionBank_WithQuestions_Should_ReturnCorrectCount()
    {
        var bankId = Guid.NewGuid();
        var bank = new QuestionBank
        {
            Id = bankId,
            Name = "Test Bank",
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.QuestionBanks.Add(bank);

        for (int i = 0; i < 5; i++)
        {
            _dbContext.Questions.Add(new Question
            {
                Id = Guid.NewGuid(),
                Text = $"Question {i}",
                Category = "Math",
                Difficulty = (byte)(i % 3 + 1),
                QuestionBankId = bankId,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _dbContext.SaveChangesAsync();

        var count = await _dbContext.Questions
            .Where(q => q.QuestionBankId == bankId)
            .CountAsync();

        count.Should().Be(5);
    }
}
