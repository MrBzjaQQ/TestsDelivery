namespace StudentManagementService.Domain.Exceptions;

public class TestAssignmentNotFoundException : Exception
{
    public Guid StudentId { get; }

    public Guid TestId { get; }

    public TestAssignmentNotFoundException(Guid studentId, Guid testId)
        : base($"Test assignment for student '{studentId}' and test '{testId}' not found")
    {
        StudentId = studentId;
        TestId = testId;
    }
}
