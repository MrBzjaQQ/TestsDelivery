namespace QuestionManagementService.Domain.Exceptions;

public class QuestionNotFoundException : Exception
{
    public Guid QuestionId { get; }

    public QuestionNotFoundException(Guid id)
        : base($"Question with id '{id}' not found")
    {
        QuestionId = id;
    }
}
