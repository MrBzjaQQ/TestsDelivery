using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using QuestionManagementService.Application.Contracts;
using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;
using QuestionManagementService.Application.Infrastructure.Database.Contract;
using QuestionManagementService.Domain.Entities;
using QuestionManagementService.Domain.Exceptions;

namespace QuestionManagementService.Application.Services;

public class QuestionBankService : IQuestionBankService
{
    private readonly IQuestionBankRepository _questionBankRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly ILogger<QuestionBankService> _logger;

    public QuestionBankService(
        IQuestionBankRepository questionBankRepository,
        IQuestionRepository questionRepository,
        ILogger<QuestionBankService> logger)
    {
        _questionBankRepository = questionBankRepository;
        _questionRepository = questionRepository;
        _logger = logger;
    }

    public async Task<QuestionBankDto> CreateQuestionBankAsync(CreateQuestionBankRequest request, CancellationToken ct)
    {
        Guard.IsNotNull(request);

        var questionBank = new QuestionBank
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            OwnerId = request.OwnerId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _questionBankRepository.AddAsync(questionBank, ct);

        _logger.LogInformation("Question bank created: {QuestionBankId}", questionBank.Id);

        return MapToDto(questionBank, 0);
    }

    public async Task<QuestionBankDto> GetQuestionBankByIdAsync(Guid id, CancellationToken ct)
    {
        var questionBank = await _questionBankRepository.GetByIdAsync(id, ct);
        if (questionBank == null)
        {
            throw new QuestionBankNotFoundException(id);
        }

        var questionsCount = await _questionBankRepository.GetQuestionsCountAsync(id, ct);

        return MapToDto(questionBank, questionsCount);
    }

    public async Task<QuestionListDto> GetQuestionsByBankIdAsync(Guid bankId, CancellationToken ct)
    {
        var bank = await _questionBankRepository.GetByIdAsync(bankId, ct);
        if (bank == null)
        {
            throw new QuestionBankNotFoundException(bankId);
        }

        var questions = await _questionRepository.GetByBankIdAsync(bankId, ct);

        return new QuestionListDto
        {
            Questions = questions.Select(MapQuestionToDto).ToList(),
            TotalCount = questions.Count
        };
    }

    private static QuestionBankDto MapToDto(QuestionBank bank, int questionsCount)
    {
        return new QuestionBankDto
        {
            Id = bank.Id,
            Name = bank.Name,
            Description = bank.Description,
            QuestionsCount = questionsCount,
            CreatedAt = bank.CreatedAt
        };
    }

    private static QuestionDto MapQuestionToDto(Question question)
    {
        return new QuestionDto
        {
            Id = question.Id,
            Text = question.Text,
            Category = question.Category,
            Difficulty = MapDifficultyToString(question.Difficulty),
            QuestionBankId = question.QuestionBankId,
            Options = question.AnswerOptions?
                .OrderBy(o => o.Ordinal)
                .Select(o => new AnswerOptionDto { Text = o.Text, IsCorrect = o.IsCorrect })
                .ToList(),
            CreatedAt = question.CreatedAt
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
