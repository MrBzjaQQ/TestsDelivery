using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using QuestionManagementService.Application.Contracts;
using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.DTOs.Responses;
using QuestionManagementService.Domain.Exceptions;
using QuestionManagementService.WebApi.Controllers;
using Xunit;

namespace QuestionManagementService.Tests.Unit.Controllers;

public class QuestionsControllerTests
{
    private readonly Mock<IQuestionService> _mockQuestionService;
    private readonly QuestionsController _controller;

    public QuestionsControllerTests()
    {
        _mockQuestionService = new Mock<IQuestionService>();
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        _controller = new QuestionsController(_mockQuestionService.Object, mockLogger.Object);
    }

    [Fact]
    public async Task Create_Should_ReturnCreatedResult()
    {
        var request = new CreateQuestionRequest
        {
            Text = "Test question",
            Category = "Math",
            Difficulty = "Easy"
        };

        var response = new QuestionDto
        {
            Id = Guid.NewGuid(),
            Text = "Test question",
            Category = "Math",
            Difficulty = "Easy",
            CreatedAt = DateTime.UtcNow
        };

        _mockQuestionService.Setup(s => s.CreateQuestionAsync(It.IsAny<CreateQuestionRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.Create(request, CancellationToken.None);

        result.Should().NotBeNull();
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var responseModel = createdResult.Value.Should().BeAssignableTo<dynamic>().Subject;
    }

    [Fact]
    public async Task GetById_Should_ReturnOkResult()
    {
        var questionId = Guid.NewGuid();
        var response = new QuestionDto
        {
            Id = questionId,
            Text = "Test question",
            Category = "Math",
            Difficulty = "Easy",
            CreatedAt = DateTime.UtcNow
        };

        _mockQuestionService.Setup(s => s.GetQuestionByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetById(questionId, CancellationToken.None);

        result.Should().NotBeNull();
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    }

    [Fact]
    public async Task Update_Should_ReturnOkResult()
    {
        var questionId = Guid.NewGuid();
        var request = new UpdateQuestionRequest
        {
            Text = "Updated question",
            Category = "Science",
            Difficulty = "Medium"
        };

        var response = new QuestionDto
        {
            Id = questionId,
            Text = "Updated question",
            Category = "Science",
            Difficulty = "Medium",
            CreatedAt = DateTime.UtcNow
        };

        _mockQuestionService.Setup(s => s.UpdateQuestionAsync(questionId, It.IsAny<UpdateQuestionRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.Update(questionId, request, CancellationToken.None);

        result.Should().NotBeNull();
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    }

    [Fact]
    public async Task Delete_Should_ReturnNoContent()
    {
        var questionId = Guid.NewGuid();

        _mockQuestionService.Setup(s => s.DeleteQuestionAsync(questionId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _controller.Delete(questionId, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetQuestions_Should_ReturnOkResult()
    {
        var response = new QuestionListDto
        {
            Questions =
            [
                new() { Id = Guid.NewGuid(), Text = "Q1", Category = "Math", Difficulty = "Easy", CreatedAt = DateTime.UtcNow }
            ],
            TotalCount = 1
        };

        _mockQuestionService.Setup(s => s.GetQuestionsAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.GetQuestions("Math", null, null, CancellationToken.None);

        result.Should().NotBeNull();
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    }
}
