namespace StudentManagementService.Domain.Entities;

public class TestProgress
{
    public Guid Id { get; init; }

    public Guid StudentId { get; init; }

    public Guid TestId { get; init; }

    public short AttemptNumber { get; init; }

    public short? Score { get; set; }

    public short MaxScore { get; set; }

    public bool IsPassed { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public List<Answer>? Answers { get; set; }

    public DateTime CreatedAt { get; init; }
}
