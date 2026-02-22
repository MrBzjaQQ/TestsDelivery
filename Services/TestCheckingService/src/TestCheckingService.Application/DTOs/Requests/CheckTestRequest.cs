namespace TestCheckingService.Application.DTOs.Requests;

public class CheckTestRequest
{
    public Guid StudentId { get; init; }

    public List<AnswerDto> Answers { get; init; } = [];
}
