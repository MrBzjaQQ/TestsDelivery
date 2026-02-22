namespace StudentManagementService.Domain.Exceptions;

public class TestNotEligibleException : Exception
{
    public List<string> Violations { get; }

    public TestNotEligibleException(string message, List<string>? violations = null)
        : base(message)
    {
        Violations = violations ?? [];
    }
}
