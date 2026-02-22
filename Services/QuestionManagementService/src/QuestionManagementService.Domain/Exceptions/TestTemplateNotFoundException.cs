namespace QuestionManagementService.Domain.Exceptions;

public class TestTemplateNotFoundException : Exception
{
    public Guid TemplateId { get; }

    public TestTemplateNotFoundException(Guid id)
        : base($"Test template with id '{id}' not found")
    {
        TemplateId = id;
    }
}
