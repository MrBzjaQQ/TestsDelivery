namespace StudentManagementService.Domain.Entities;

public class StudyGroup
{
    public Guid Id { get; init; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public short StartYear { get; set; }

    public short EndYear { get; set; }

    public DateTime CreatedAt { get; init; }
}
