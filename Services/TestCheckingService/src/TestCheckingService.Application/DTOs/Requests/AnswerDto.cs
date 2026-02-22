namespace TestCheckingService.Application.DTOs.Requests;

public class AnswerDto
{
    public Guid QuestionId { get; init; }

    public Guid? SelectedOptionId { get; init; }

    public string? TextAnswer { get; init; }
}
