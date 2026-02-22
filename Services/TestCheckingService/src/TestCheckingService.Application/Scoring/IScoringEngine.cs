using TestCheckingService.Application.DTOs.Requests;
using TestCheckingService.Application.DTOs.Responses;

namespace TestCheckingService.Application.Scoring;

public interface IScoringEngine
{
    Task<ScoringResult> ScoreTestAsync(
        Guid testId,
        List<AnswerDto> answers,
        int passPercentage,
        CancellationToken ct);
}

public class ScoringResult
{
    public int TotalScore { get; init; }

    public int MaxScore { get; init; }

    public double Percentage { get; init; }

    public bool IsPassed { get; init; }

    public List<AnswerScoreResult> AnswerResults { get; init; } = [];
}

public class AnswerScoreResult
{
    public Guid QuestionId { get; init; }

    public Guid? SelectedOptionId { get; init; }

    public bool IsCorrect { get; init; }

    public int PointsEarned { get; init; }

    public Guid? CorrectOptionId { get; init; }
}
