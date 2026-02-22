namespace QuestionManagementService.Domain.Entities;

public class TestTemplate
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int DefaultDuration { get; set; }

    public byte DefaultPassingScore { get; set; }

    public string? Configuration { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
