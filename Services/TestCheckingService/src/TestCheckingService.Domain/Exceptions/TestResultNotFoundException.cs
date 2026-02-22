namespace TestCheckingService.Domain.Exceptions;

public class TestResultNotFoundException : Exception
{
    public Guid TestId { get; }

    public Guid StudentId { get; }

    public TestResultNotFoundException(Guid testId, Guid studentId)
        : base($"Test result for test '{testId}' and student '{studentId}' not found")
    {
        TestId = testId;
        StudentId = studentId;
    }

    public TestResultNotFoundException(Guid testId)
        : base($"Test result for test '{testId}' not found")
    {
        TestId = testId;
    }
}
