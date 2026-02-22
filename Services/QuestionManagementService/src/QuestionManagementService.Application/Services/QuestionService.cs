using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using QuestionManagementService.Application.Contracts;
using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;
using QuestionManagementService.Application.Infrastructure.Database.Contract;
using QuestionManagementService.Domain.Entities;
using QuestionManagementService.Domain.Exceptions;

namespace QuestionManagementService.Application.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuestionBankRepository _questionBankRepository;
    private readonly ILogger<QuestionService> _logger;

    public QuestionService(
        IQuestionRepository questionRepository,
        IQuestionBankRepository questionBankRepository,
        ILogger<QuestionService> logger)
    {
        _questionRepository = questionRepository;
        _questionBankRepository = questionBankRepository;
        _logger = logger;
    }

    public async Task<QuestionDto> CreateQuestionAsync(CreateQuestionRequest request, CancellationToken ct)
    {
        Guard.IsNotNull(request);

        if (request.QuestionBankId.HasValue)
        {
            var bank = await _questionBankRepository.GetByIdAsync(request.QuestionBankId.Value, ct);
            if (bank == null)
            {
                throw new QuestionBankNotFoundException(request.QuestionBankId.Value);
            }
        }

        var difficulty = MapDifficulty(request.Difficulty);
        var question = new Question
        {
            Id = Guid.NewGuid(),
            Text = request.Text,
            Category = request.Category,
            Difficulty = difficulty,
            QuestionBankId = request.QuestionBankId ?? Guid.Empty,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        if (request.Options != null)
        {
            var ordinal = 0;
            foreach (var opt in request.Options)
            {
                question.AnswerOptions.Add(new AnswerOption
                {
                    Id = Guid.NewGuid(),
                    QuestionId = question.Id,
                    Text = opt.Text,
                    IsCorrect = opt.IsCorrect,
                    Ordinal = ordinal++
                });
            }
        }

        await _questionRepository.AddAsync(question, ct);

        _logger.LogInformation("Question created: {QuestionId}", question.Id);

        return MapToDto(question);
    }

    public async Task<QuestionDto> GetQuestionByIdAsync(Guid id, CancellationToken ct)
    {
        var question = await _questionRepository.GetByIdAsync(id, ct);
        if (question == null)
        {
            throw new QuestionNotFoundException(id);
        }

        return MapToDto(question);
    }

    public async Task<QuestionDto> UpdateQuestionAsync(Guid id, UpdateQuestionRequest request, CancellationToken ct)
    {
        Guard.IsNotNull(request);

        var question = await _questionRepository.GetByIdAsync(id, ct);
        if (question == null)
        {
            throw new QuestionNotFoundException(id);
        }

        var difficulty = MapDifficulty(request.Difficulty);
        var updatedQuestion = new Question
        {
            Id = question.Id,
            Text = request.Text,
            Category = request.Category,
            Difficulty = difficulty,
            QuestionBankId = question.QuestionBankId,
            CreatedAt = question.CreatedAt,
            IsDeleted = question.IsDeleted
        };

        if (request.Options != null)
        {
            var ordinal = 0;
            foreach (var opt in request.Options)
            {
                updatedQuestion.AnswerOptions.Add(new AnswerOption
                {
                    Id = Guid.NewGuid(),
                    QuestionId = updatedQuestion.Id,
                    Text = opt.Text,
                    IsCorrect = opt.IsCorrect,
                    Ordinal = ordinal++
                });
            }
        }

        await _questionRepository.UpdateAsync(updatedQuestion, ct);

        _logger.LogInformation("Question updated: {QuestionId}", id);

        return MapToDto(updatedQuestion);
    }

    public async Task DeleteQuestionAsync(Guid id, CancellationToken ct)
    {
        var question = await _questionRepository.GetByIdAsync(id, ct);
        if (question == null)
        {
            throw new QuestionNotFoundException(id);
        }

        await _questionRepository.DeleteAsync(id, ct);

        _logger.LogInformation("Question deleted: {QuestionId}", id);
    }

    public async Task<QuestionListDto> GetQuestionsAsync(string? category, string? difficulty, Guid? questionBankId, CancellationToken ct)
    {
        var questions = await _questionRepository.GetByFilterAsync(category, difficulty, questionBankId, ct);

        return new QuestionListDto
        {
            Questions = questions.Select(MapToDto).ToList(),
            TotalCount = questions.Count
        };
    }

    private static QuestionDto MapToDto(Question question)
    {
        return new QuestionDto
        {
            Id = question.Id,
            Text = question.Text,
            Category = question.Category,
            Difficulty = MapDifficultyToString(question.Difficulty),
            QuestionBankId = question.QuestionBankId == Guid.Empty ? null : question.QuestionBankId,
            Options = question.AnswerOptions?
                .OrderBy(o => o.Ordinal)
                .Select(o => new AnswerOptionDto { Text = o.Text, IsCorrect = o.IsCorrect })
                .ToList(),
            CreatedAt = question.CreatedAt
        };
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

    private static string MapDifficultyToString(byte difficulty)
    {
        return difficulty switch
        {
            1 => "Easy",
            2 => "Medium",
            3 => "Hard",
            _ => "Easy"
        };
    }
}
