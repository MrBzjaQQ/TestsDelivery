namespace StudentManagementService.Application.DTOs.Responses;

public class TestAssignmentDto
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid TestId { get; set; }

    public string? TestTitle { get; set; }

    public DateTime AssignedAt { get; set; }

    public DateTime? Deadline { get; set; }

    public string Status { get; set; } = string.Empty;

    public short AttemptsAllowed { get; set; }

    public short AttemptsUsed { get; set; }
}
