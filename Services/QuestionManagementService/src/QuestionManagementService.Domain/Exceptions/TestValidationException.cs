namespace QuestionManagementService.Domain.Exceptions;

public class TestValidationException : Exception
{
    public List<TestValidationViolation> Violations { get; }

    public TestValidationException(string message, List<TestValidationViolation>? violations = null)
        : base(message)
    {
        Violations = violations ?? [];
    }
}

public record TestValidationViolation(string Field, string Message, string Code);
