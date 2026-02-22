namespace TestCheckingService.Application.DTOs.Responses;

public class AnswerResultDto
{
    public Guid QuestionId { get; init; }

    public Guid? SelectedOptionId { get; init; }

    public bool IsCorrect { get; init; }

    public int PointsEarned { get; init; }

    public Guid? CorrectOptionId { get; init; }
}
