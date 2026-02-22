namespace QuestionManagementService.Domain.Entities;

public class TestQuestion
{
    public Guid TestId { get; init; }

    public Test? Test { get; init; }

    public Guid QuestionId { get; init; }

    public Question? Question { get; init; }

    public int Ordinal { get; init; }
}
