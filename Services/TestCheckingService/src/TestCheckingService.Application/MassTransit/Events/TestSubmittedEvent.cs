namespace TestCheckingService.Application.MassTransit.Events;

public interface TestSubmittedEvent
{
    Guid TestId { get; }

    Guid StudentId { get; }

    string StudentName { get; }

    List<AnswerSubmission> Answers { get; }

    DateTime SubmittedAt { get; }

    int AttemptNumber { get; }
}

public class AnswerSubmission
{
    public Guid QuestionId { get; init; }

    public Guid? SelectedOptionId { get; init; }

    public string? TextAnswer { get; init; }
}
