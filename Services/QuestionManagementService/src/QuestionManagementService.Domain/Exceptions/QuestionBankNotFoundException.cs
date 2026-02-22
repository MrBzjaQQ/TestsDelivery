namespace QuestionManagementService.Domain.Exceptions;

public class QuestionBankNotFoundException : Exception
{
    public Guid QuestionBankId { get; }

    public QuestionBankNotFoundException(Guid id)
        : base($"Question bank with id '{id}' not found")
    {
        QuestionBankId = id;
    }
}
