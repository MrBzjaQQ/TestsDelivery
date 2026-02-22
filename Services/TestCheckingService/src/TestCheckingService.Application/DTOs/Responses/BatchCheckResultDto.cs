namespace TestCheckingService.Application.DTOs.Responses;

public class BatchCheckResultDto
{
    public int TotalChecks { get; init; }

    public int SuccessfulChecks { get; init; }

    public int FailedChecks { get; init; }

    public List<TestResultDto> Results { get; init; } = [];
}
