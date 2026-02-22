using Microsoft.EntityFrameworkCore;
using QuestionManagementService.Application.Infrastructure.Database.Contract;
using QuestionManagementService.Domain.Entities;
using QuestionManagementService.Infrastructure.Database.Context;

namespace QuestionManagementService.Infrastructure.Database.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly IQuestionDbContext _context;

    public QuestionRepository(IQuestionDbContext context)
    {
        _context = context;
    }

    public async Task<Question?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Questions
            .Include(q => q.AnswerOptions)
            .FirstOrDefaultAsync(q => q.Id == id, ct);
    }

    public async Task<List<Question>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Questions
            .Include(q => q.AnswerOptions)
            .ToListAsync(ct);
    }

    public async Task<List<Question>> GetByFilterAsync(string? category, string? difficulty, Guid? questionBankId, CancellationToken ct)
    {
        var query = _context.Questions.Include(q => q.AnswerOptions).AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(q => q.Category.ToLower() == category.ToLower());
        }

        if (!string.IsNullOrEmpty(difficulty))
        {
            var diffValue = MapDifficulty(difficulty);
            query = query.Where(q => q.Difficulty == diffValue);
        }

        if (questionBankId.HasValue)
        {
            query = query.Where(q => q.QuestionBankId == questionBankId.Value);
        }

        return await query.ToListAsync(ct);
    }

    public async Task<List<Question>> GetByBankIdAsync(Guid bankId, CancellationToken ct)
    {
        return await _context.Questions
            .Include(q => q.AnswerOptions)
            .Where(q => q.QuestionBankId == bankId)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Question question, CancellationToken ct)
    {
        await _context.Questions.AddAsync(question, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Question question, CancellationToken ct)
    {
        var existingOptions = await _context.AnswerOptions
            .Where(ao => ao.QuestionId == question.Id)
            .ToListAsync(ct);

        _context.AnswerOptions.RemoveRange(existingOptions);

        var trackedEntity = await _context.Questions.FindAsync(question.Id, ct);
        if (trackedEntity != null)
        {
            trackedEntity.Text = question.Text;
            trackedEntity.Category = question.Category;
            trackedEntity.Difficulty = question.Difficulty;

            foreach (var option in question.AnswerOptions)
            {
                await _context.AnswerOptions.AddAsync(option, ct);
            }
        }

        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var question = await _context.Questions.FindAsync(id, ct);
        if (question != null)
        {
            question.IsDeleted = true;
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<int> GetCountByBankIdAsync(Guid bankId, CancellationToken ct)
    {
        return await _context.Questions
            .Where(q => q.QuestionBankId == bankId)
            .CountAsync(ct);
    }

    private static byte MapDifficulty(string difficulty)
    {
        return difficulty.ToLowerInvariant() switch
        {
            "easy" => 1,
            "medium" => 2,
            "hard" => 3,
            _ => 1
        };
    }
}
