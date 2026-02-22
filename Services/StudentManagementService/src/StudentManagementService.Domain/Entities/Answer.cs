namespace StudentManagementService.Domain.Entities;

public class Answer
{
    public Guid QuestionId { get; init; }

    public Guid? SelectedOptionId { get; set; }

    public string? TextAnswer { get; set; }

    public bool? IsCorrect { get; set; }

    public short? PointsEarned { get; set; }
}
