namespace BffPortalService.Domain.ValueObjects;

public record PortalDataKey
{
    public string Value { get; }

    private PortalDataKey(string value)
    {
        Value = value;
    }

    public static PortalDataKey StudentProfile(Guid studentId) =>
        new($"student-profile:{studentId}");

    public static PortalDataKey AvailableTests(Guid userId) =>
        new($"available-tests:{userId}");

    public static PortalDataKey GroupAnalytics(Guid groupId) =>
        new($"group-analytics:{groupId}");

    public static PortalDataKey TestQuestions(Guid testId) =>
        new($"test-questions:{testId}");

    public override string ToString() => Value;
}
