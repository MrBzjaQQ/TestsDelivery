namespace BffPortalService.Application.DTOs.Requests;

public record SubmitTestRequest
{
    public Guid TestId { get; init; }

    public Guid StudentId { get; init; }

    public List<AnswerDto> Answers { get; init; } = [];
}

public record AnswerDto
{
    public Guid QuestionId { get; init; }

    public List<Guid> SelectedOptionIds { get; init; } = [];

    public string? TextAnswer { get; init; }
}
