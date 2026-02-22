namespace QuestionManagementService.Domain.Entities;

public class AnswerOption
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }

    public Question? Question { get; set; }

    public string Text { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }

    public int Ordinal { get; set; }
}
