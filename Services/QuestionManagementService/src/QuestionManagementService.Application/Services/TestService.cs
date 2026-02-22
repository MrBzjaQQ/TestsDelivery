using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using QuestionManagementService.Application.Contracts;
using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;
using QuestionManagementService.Application.Infrastructure.Database.Contract;
using QuestionManagementService.Domain.Entities;
using QuestionManagementService.Domain.Exceptions;

namespace QuestionManagementService.Application.Services;

public class TestService : ITestService
{
    private readonly ITestRepository _testRepository;
    private readonly IQuestionBankRepository _questionBankRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly ITestTemplateRepository _templateRepository;
    private readonly ILogger<TestService> _logger;

    public TestService(
        ITestRepository testRepository,
        IQuestionBankRepository questionBankRepository,
        IQuestionRepository questionRepository,
        ITestTemplateRepository templateRepository,
        ILogger<TestService> logger)
    {
        _testRepository = testRepository;
        _questionBankRepository = questionBankRepository;
        _questionRepository = questionRepository;
        _templateRepository = templateRepository;
        _logger = logger;
    }

    public async Task<TestDto> CreateTestAsync(CreateTestRequest request, CancellationToken ct)
    {
        Guard.IsNotNull(request);

        var bank = await _questionBankRepository.GetByIdAsync(request.QuestionBankId, ct);
        if (bank == null)
        {
            throw new QuestionBankNotFoundException(request.QuestionBankId);
        }

        if (request.TemplateId.HasValue)
        {
            var template = await _templateRepository.GetByIdAsync(request.TemplateId.Value, ct);
            if (template == null)
            {
                throw new TestTemplateNotFoundException(request.TemplateId.Value);
            }
        }

        var test = new Test
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            QuestionBankId = request.QuestionBankId,
            TemplateId = request.TemplateId,
            DurationMinutes = request.DurationMinutes,
            PassingScore = request.PassingScore,
            MaxAttempts = request.MaxAttempts,
            Status = "Draft",
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _testRepository.AddAsync(test, ct);

        _logger.LogInformation("Test created: {TestId}", test.Id);

        return MapToDto(test);
    }

    public async Task<TestDto> GetTestByIdAsync(Guid id, CancellationToken ct)
    {
        var test = await _testRepository.GetByIdAsync(id, ct);
        if (test == null)
        {
            throw new TestNotFoundException(id);
        }

        return MapToDto(test);
    }

    public async Task<TestListDto> GetTestsByTemplateIdAsync(Guid templateId, CancellationToken ct)
    {
        var tests = await _testRepository.GetByTemplateIdAsync(templateId, ct);

        return new TestListDto
        {
            Tests = tests.Select(MapToDto).ToList(),
            TotalCount = tests.Count
        };
    }

    public async Task<TestDto> CopyTestAsync(Guid id, CancellationToken ct)
    {
        var originalTest = await _testRepository.GetByIdAsync(id, ct);
        if (originalTest == null)
        {
            throw new TestNotFoundException(id);
        }

        var copiedTest = new Test
        {
            Id = Guid.NewGuid(),
            Title = $"{originalTest.Title} (Copy)",
            Description = originalTest.Description,
            QuestionBankId = originalTest.QuestionBankId,
            TemplateId = originalTest.TemplateId,
            DurationMinutes = originalTest.DurationMinutes,
            PassingScore = originalTest.PassingScore,
            MaxAttempts = originalTest.MaxAttempts,
            Status = "Draft",
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        foreach (var testQuestion in originalTest.TestQuestions)
        {
            copiedTest.TestQuestions.Add(new TestQuestion
            {
                TestId = copiedTest.Id,
                QuestionId = testQuestion.QuestionId,
                Ordinal = testQuestion.Ordinal
            });
        }

        await _testRepository.AddAsync(copiedTest, ct);

        _logger.LogInformation("Test copied: {NewTestId} from {OriginalTestId}", copiedTest.Id, id);

        return MapToDto(copiedTest);
    }

    public async Task<TestDto> GenerateTestFromBankAsync(Guid bankId, GenerateTestRequest request, CancellationToken ct)
    {
        Guard.IsNotNull(request);

        var bank = await _questionBankRepository.GetByIdAsync(bankId, ct);
        if (bank == null)
        {
            throw new QuestionBankNotFoundException(bankId);
        }

        var questions = await _questionRepository.GetByBankIdAsync(bankId, ct);

        if (!string.IsNullOrEmpty(request.DifficultyFilter))
        {
            var targetDifficulty = MapDifficulty(request.DifficultyFilter);
            questions = questions.Where(q => q.Difficulty == targetDifficulty).ToList();
        }

        if (questions.Count < request.QuestionCount)
        {
            throw new TestValidationException($"Not enough questions in bank. Requested: {request.QuestionCount}, Available: {questions.Count}");
        }

        var selectedQuestions = questions
            .OrderBy(_ => Guid.NewGuid())
            .Take(request.QuestionCount)
            .ToList();

        var test = new Test
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = $"Generated test from bank: {bank.Name}",
            QuestionBankId = bankId,
            DurationMinutes = 60,
            PassingScore = 70,
            MaxAttempts = 3,
            Status = "Draft",
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        var ordinal = 0;
        foreach (var question in selectedQuestions)
        {
            test.TestQuestions.Add(new TestQuestion
            {
                TestId = test.Id,
                QuestionId = question.Id,
                Ordinal = ordinal++
            });
        }

        await _testRepository.AddAsync(test, ct);

        _logger.LogInformation("Test generated: {TestId} with {Count} questions", test.Id, request.QuestionCount);

        return MapToDtoWithQuestions(test, selectedQuestions);
    }

    private static TestDto MapToDto(Test test)
    {
        return new TestDto
        {
            Id = test.Id,
            Title = test.Title,
            Description = test.Description,
            QuestionBankId = test.QuestionBankId,
            TemplateId = test.TemplateId,
            DurationMinutes = test.DurationMinutes,
            PassingScore = test.PassingScore,
            MaxAttempts = test.MaxAttempts,
            Status = test.Status,
            CreatedAt = test.CreatedAt
        };
    }

    private static TestDto MapToDtoWithQuestions(Test test, List<Question> questions)
    {
        return new TestDto
        {
            Id = test.Id,
            Title = test.Title,
            Description = test.Description,
            QuestionBankId = test.QuestionBankId,
            TemplateId = test.TemplateId,
            DurationMinutes = test.DurationMinutes,
            PassingScore = test.PassingScore,
            MaxAttempts = test.MaxAttempts,
            Status = test.Status,
            Questions = questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                Category = q.Category,
                Difficulty = MapDifficultyToString(q.Difficulty),
                QuestionBankId = q.QuestionBankId,
                Options = q.AnswerOptions?
                    .OrderBy(o => o.Ordinal)
                    .Select(o => new AnswerOptionDto { Text = o.Text, IsCorrect = o.IsCorrect })
                    .ToList(),
                CreatedAt = q.CreatedAt
            }).ToList(),
            CreatedAt = test.CreatedAt
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
