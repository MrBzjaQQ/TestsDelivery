namespace QuestionManagementService.Domain.Exceptions;

public class TestNotFoundException : Exception
{
    public Guid TestId { get; }

    public TestNotFoundException(Guid id)
        : base($"Test with id '{id}' not found")
    {
        TestId = id;
    }
}
