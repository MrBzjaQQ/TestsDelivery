namespace TestCheckingService.Application.DTOs.Requests;

public class BatchCheckRequest
{
    public List<BatchCheckItem> Checks { get; init; } = [];
}

public class BatchCheckItem
{
    public Guid TestId { get; init; }

    public Guid StudentId { get; init; }

    public List<AnswerDto> Answers { get; init; } = [];
}
