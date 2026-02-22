namespace QuestionManagementService.Domain.Entities;

public class QuestionBank
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public Guid OwnerId { get; init; }

    public ICollection<Question> Questions { get; init; } = [];

    public DateTime CreatedAt { get; init; }

    public bool IsDeleted { get; set; }
}
