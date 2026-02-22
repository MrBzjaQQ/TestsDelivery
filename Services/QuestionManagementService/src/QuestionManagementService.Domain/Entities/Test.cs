namespace QuestionManagementService.Domain.Entities;

public class Test
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Description { get; init; }

    public Guid QuestionBankId { get; init; }

    public QuestionBank? QuestionBank { get; init; }

    public Guid? TemplateId { get; init; }

    public TestTemplate? Template { get; init; }

    public int DurationMinutes { get; init; }

    public byte PassingScore { get; init; }

    public int MaxAttempts { get; init; }

    public string Status { get; set; } = "Draft";

    public ICollection<TestQuestion> TestQuestions { get; init; } = [];

    public DateTime CreatedAt { get; init; }

    public bool IsDeleted { get; set; }
}
