namespace TestCheckingService.Domain.Exceptions;

public class TestNotEligibleException : Exception
{
    public List<TestEligibilityViolation> Violations { get; }

    public TestNotEligibleException(string message, List<TestEligibilityViolation>? violations = null)
        : base(message)
    {
        Violations = violations ?? [];
    }
}

public record TestEligibilityViolation(string Field, string Message, string Code);
