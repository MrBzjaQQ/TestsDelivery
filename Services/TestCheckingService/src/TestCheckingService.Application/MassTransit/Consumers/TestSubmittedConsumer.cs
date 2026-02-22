using MassTransit;
using Microsoft.Extensions.Logging;
using TestCheckingService.Application.Contracts;
using TestCheckingService.Application.DTOs.Requests;
using TestCheckingService.Application.MassTransit.Events;

namespace TestCheckingService.Application.MassTransit.Consumers;

public class TestSubmittedConsumer : IConsumer<TestSubmittedEvent>
{
    private readonly ITestCheckService _testCheckService;
    private readonly ILogger<TestSubmittedConsumer> _logger;

    public TestSubmittedConsumer(
        ITestCheckService testCheckService,
        ILogger<TestSubmittedConsumer> logger)
    {
        _testCheckService = testCheckService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TestSubmittedEvent> context)
    {
        var @event = context.Message;

        _logger.LogInformation(
            "Processing test submission for test {TestId}, student {StudentId}",
            @event.TestId, @event.StudentId);

        var answers = @event.Answers.Select(a => new AnswerDto
        {
            QuestionId = a.QuestionId,
            SelectedOptionId = a.SelectedOptionId,
            TextAnswer = a.TextAnswer
        }).ToList();

        var result = await _testCheckService.CheckTestAsync(
            @event.TestId,
            @event.StudentId,
            answers,
            context.CancellationToken);

        await context.Publish(new TestResultReceived
        {
            TestId = @event.TestId,
            StudentId = @event.StudentId,
            StudentName = @event.StudentName,
            StudentEmail = string.Empty,
            Score = result.Score,
            MaxScore = result.MaxScore,
            Percentage = result.Percentage,
            IsPassed = result.IsPassed,
            PassedThreshold = result.PassedThreshold,
            PassedDate = result.PassedDate ?? DateTime.UtcNow,
            AttemptNumber = result.AttemptNumber
        }, context.CancellationToken);

        _logger.LogInformation(
            "Test result published for test {TestId}, student {StudentId}",
            @event.TestId, @event.StudentId);
    }
}
