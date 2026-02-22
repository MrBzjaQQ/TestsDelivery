using QuestionManagementService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace QuestionManagementService.Infrastructure.Database.Context;

public interface IQuestionDbContext
{
    DbSet<Question> Questions { get; }

    DbSet<QuestionBank> QuestionBanks { get; }

    DbSet<Test> Tests { get; }

    DbSet<TestTemplate> TestTemplates { get; }

    DbSet<AnswerOption> AnswerOptions { get; }

    DbSet<TestQuestion> TestQuestions { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}
