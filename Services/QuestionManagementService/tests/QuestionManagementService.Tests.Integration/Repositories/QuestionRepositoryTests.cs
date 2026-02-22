using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using QuestionManagementService.Domain.Entities;
using QuestionManagementService.Infrastructure.Database.Context;
using QuestionManagementService.Tests.Integration.TestInfrastructure;
using Xunit;

namespace QuestionManagementService.Tests.Integration.Repositories;

[Collection("Database")]
public class QuestionRepositoryTests : IAsyncLifetime
{
    private readonly DatabaseFixture _fixture;
    private QuestionDbContext _dbContext = null!;

    public QuestionRepositoryTests(DatabaseFixture fixture)
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
    public async Task GetByIdAsync_Should_ReturnQuestion_When_Exists()
    {
        var bankId = Guid.NewGuid();
        var questionId = Guid.NewGuid();

        var bank = new QuestionBank
        {
            Id = bankId,
            Name = "Test Bank",
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        var question = new Question
        {
            Id = questionId,
            Text = "Test question",
            Category = "Math",
            Difficulty = 1,
            QuestionBankId = bankId,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.QuestionBanks.Add(bank);
        _dbContext.Questions.Add(question);
        await _dbContext.SaveChangesAsync();

        var repository = new QuestionRepository(_dbContext);
        var result = await repository.GetByIdAsync(questionId, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Text.Should().Be("Test question");
        result.Category.Should().Be("Math");
    }

    [Fact]
    public async Task AddAsync_Should_AddQuestion()
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
        await _dbContext.SaveChangesAsync();

        var question = new Question
        {
            Id = Guid.NewGuid(),
            Text = "New question",
            Category = "Science",
            Difficulty = 2,
            QuestionBankId = bankId,
            CreatedAt = DateTime.UtcNow
        };

        var repository = new QuestionRepository(_dbContext);
        await repository.AddAsync(question, CancellationToken.None);

        var savedQuestion = await _dbContext.Questions.FindAsync(question.Id);
        savedQuestion.Should().NotBeNull();
        savedQuestion!.Text.Should().Be("New question");
    }

    [Fact]
    public async Task GetByFilterAsync_Should_FilterByCategory()
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

        _dbContext.Questions.AddRange(
            new Question { Id = Guid.NewGuid(), Text = "Q1", Category = "Math", Difficulty = 1, QuestionBankId = bankId, CreatedAt = DateTime.UtcNow },
            new Question { Id = Guid.NewGuid(), Text = "Q2", Category = "Science", Difficulty = 1, QuestionBankId = bankId, CreatedAt = DateTime.UtcNow },
            new Question { Id = Guid.NewGuid(), Text = "Q3", Category = "Math", Difficulty = 2, QuestionBankId = bankId, CreatedAt = DateTime.UtcNow }
        );
        await _dbContext.SaveChangesAsync();

        var repository = new QuestionRepository(_dbContext);
        var result = await repository.GetByFilterAsync("Math", null, null, CancellationToken.None);

        result.Should().HaveCount(2);
        result.All(q => q.Category == "Math").Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Should_SoftDeleteQuestion()
    {
        var questionId = Guid.NewGuid();
        var bankId = Guid.NewGuid();

        var bank = new QuestionBank
        {
            Id = bankId,
            Name = "Test Bank",
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        var question = new Question
        {
            Id = questionId,
            Text = "To delete",
            Category = "Math",
            Difficulty = 1,
            QuestionBankId = bankId,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.QuestionBanks.Add(bank);
        _dbContext.Questions.Add(question);
        await _dbContext.SaveChangesAsync();

        var repository = new QuestionRepository(_dbContext);
        await repository.DeleteAsync(questionId, CancellationToken.None);

        var deletedQuestion = await _dbContext.Questions
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(q => q.Id == questionId);

        deletedQuestion.Should().NotBeNull();
        deletedQuestion!.IsDeleted.Should().BeTrue();
    }
}

file class QuestionRepository(QuestionDbContext context) : IQuestionRepository
{
    public Task<Question?> GetByIdAsync(Guid id, CancellationToken ct)
        => context.Questions.Include(q => q.AnswerOptions).FirstOrDefaultAsync(q => q.Id == id, ct);

    public Task<List<Question>> GetAllAsync(CancellationToken ct)
        => context.Questions.Include(q => q.AnswerOptions).ToListAsync(ct);

    public async Task<List<Question>> GetByFilterAsync(string? category, string? difficulty, Guid? questionBankId, CancellationToken ct)
    {
        var query = context.Questions.AsQueryable();
        if (!string.IsNullOrEmpty(category))
            query = query.Where(q => q.Category.ToLower() == category.ToLower());
        return await query.ToListAsync(ct);
    }

    public Task<List<Question>> GetByBankIdAsync(Guid bankId, CancellationToken ct)
        => context.Questions.Where(q => q.QuestionBankId == bankId).ToListAsync(ct);

    public async Task AddAsync(Question question, CancellationToken ct)
    {
        await context.Questions.AddAsync(question, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Question question, CancellationToken ct)
    {
        context.Questions.Update(question);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var question = await context.Questions.FindAsync(id, ct);
        if (question != null)
        {
            question.IsDeleted = true;
            await context.SaveChangesAsync(ct);
        }
    }

    public Task<int> GetCountByBankIdAsync(Guid bankId, CancellationToken ct)
        => context.Questions.Where(q => q.QuestionBankId == bankId).CountAsync(ct);
}

file interface IQuestionRepository
{
    Task<Question?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Question>> GetAllAsync(CancellationToken ct);
    Task<List<Question>> GetByFilterAsync(string? category, string? difficulty, Guid? questionBankId, CancellationToken ct);
    Task<List<Question>> GetByBankIdAsync(Guid bankId, CancellationToken ct);
    Task AddAsync(Question question, CancellationToken ct);
    Task UpdateAsync(Question question, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
    Task<int> GetCountByBankIdAsync(Guid bankId, CancellationToken ct);
}
