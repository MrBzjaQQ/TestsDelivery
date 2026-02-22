namespace StudentManagementService.Domain.Exceptions;

public class StudentAlreadyEnrolledException : Exception
{
    public Guid StudentId { get; }

    public Guid TestId { get; }

    public StudentAlreadyEnrolledException(Guid studentId, Guid testId)
        : base($"Student '{studentId}' is already enrolled in test '{testId}'")
    {
        StudentId = studentId;
        TestId = testId;
    }
}
