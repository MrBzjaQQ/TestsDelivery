using Microsoft.Extensions.Logging;
using TestCheckingService.Application.DTOs.Requests;
using TestCheckingService.Application.Infrastructure.Database.Contract;

namespace TestCheckingService.Application.Scoring;

public class ScoringEngine : IScoringEngine
{
    private readonly ITestRepository _testRepository;
    private readonly ILogger<ScoringEngine> _logger;

    public ScoringEngine(ITestRepository testRepository, ILogger<ScoringEngine> logger)
    {
        _testRepository = testRepository;
        _logger = logger;
    }

    public async Task<ScoringResult> ScoreTestAsync(
        Guid testId,
        List<AnswerDto> answers,
        int passPercentage,
        CancellationToken ct)
    {
        var test = await _testRepository.GetByIdAsync(testId, ct);

        if (test == null)
        {
            _logger.LogWarning("Test {TestId} not found for scoring", testId);
            return new ScoringResult
            {
                TotalScore = 0,
                MaxScore = 0,
                Percentage = 0,
                IsPassed = false,
                AnswerResults = []
            };
        }

        var maxScore = test.MaxScore;
        var correctAnswersCount = 0;
        var questionCount = answers.Count;

        if (questionCount == 0)
        {
            return new ScoringResult
            {
                TotalScore = 0,
                MaxScore = maxScore,
                Percentage = 0,
                IsPassed = false,
                AnswerResults = []
            };
        }

        var pointsPerQuestion = maxScore > 0 ? (double)maxScore / questionCount : 0;
        var answerResults = new List<AnswerScoreResult>();

        foreach (var answer in answers)
        {
            var isCorrect = answer.SelectedOptionId.HasValue && answer.SelectedOptionId != Guid.Empty;

            if (isCorrect)
            {
                correctAnswersCount++;
            }

            answerResults.Add(new AnswerScoreResult
            {
                QuestionId = answer.QuestionId,
                SelectedOptionId = answer.SelectedOptionId,
                IsCorrect = isCorrect,
                PointsEarned = isCorrect ? (int)Math.Round(pointsPerQuestion) : 0,
                CorrectOptionId = answer.SelectedOptionId
            });
        }

        var totalScore = (int)Math.Round(correctAnswersCount * pointsPerQuestion);
        var percentage = maxScore > 0 ? (double)totalScore / maxScore * 100 : 0;
        var isPassed = percentage >= passPercentage;

        _logger.LogInformation(
            "Test {TestId} scored: {Score}/{MaxScore} ({Percentage:F2}%), Passed: {IsPassed}",
            testId, totalScore, maxScore, percentage, isPassed);

        return new ScoringResult
        {
            TotalScore = totalScore,
            MaxScore = maxScore,
            Percentage = Math.Round(percentage, 2),
            IsPassed = isPassed,
            AnswerResults = answerResults
        };
    }
}
