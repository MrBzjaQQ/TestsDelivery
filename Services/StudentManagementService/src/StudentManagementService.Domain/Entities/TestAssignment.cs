namespace StudentManagementService.Domain.Entities;

public class TestAssignment
{
    public Guid Id { get; init; }

    public Guid StudentId { get; init; }

    public Guid TestId { get; init; }

    public DateTime AssignedAt { get; init; }

    public DateTime? Deadline { get; set; }

    public TestAssignmentStatus Status { get; set; } = TestAssignmentStatus.Assigned;

    public short AttemptsAllowed { get; set; } = 3;

    public short AttemptsUsed { get; set; } = 0;

    public DateTime CreatedAt { get; init; }
}
