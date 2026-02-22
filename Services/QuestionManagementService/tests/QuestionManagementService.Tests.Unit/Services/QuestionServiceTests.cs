using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using QuestionManagementService.Application.DTOs.Requests;
using QuestionManagementService.Application.Infrastructure.Database.Contract;
using QuestionManagementService.Application.Services;
using QuestionManagementService.Domain.Entities;
using QuestionManagementService.Domain.Exceptions;
using Xunit;

namespace QuestionManagementService.Tests.Unit.Services;

public class QuestionServiceTests
{
    private readonly Mock<IQuestionRepository> _mockQuestionRepository;
    private readonly Mock<IQuestionBankRepository> _mockQuestionBankRepository;
    private readonly Mock<ILogger<QuestionService>> _mockLogger;
    private readonly QuestionService _service;

    public QuestionServiceTests()
    {
        _mockQuestionRepository = new Mock<IQuestionRepository>();
        _mockQuestionBankRepository = new Mock<IQuestionBankRepository>();
        _mockLogger = new Mock<ILogger<QuestionService>>();
        _service = new QuestionService(
            _mockQuestionRepository.Object,
            _mockQuestionBankRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CreateQuestionAsync_Should_ReturnSuccess_When_Valid()
    {
        var request = new CreateQuestionRequest
        {
            Text = "Test question",
            Category = "Math",
            Difficulty = "Medium",
            QuestionBankId = Guid.NewGuid(),
            Options =
            [
                new() { Text = "Option A", IsCorrect = true },
                new() { Text = "Option B", IsCorrect = false }
            ]
        };

        var bank = new QuestionBank { Id = request.QuestionBankId.Value, Name = "Math Bank" };
        _mockQuestionBankRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bank);

        _mockQuestionRepository.Setup(r => r.AddAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateQuestionAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Text.Should().Be(request.Text);
        result.Category.Should().Be(request.Category);
        result.Difficulty.Should().Be("Medium");
        _mockQuestionRepository.Verify(r => r.AddAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateQuestionAsync_Should_ThrowQuestionBankNotFoundException_When_BankNotExists()
    {
        var request = new CreateQuestionRequest
        {
            Text = "Test question",
            Category = "Math",
            Difficulty = "Medium",
            QuestionBankId = Guid.NewGuid()
        };

        _mockQuestionBankRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((QuestionBank?)null);

        await Assert.ThrowsAsync<QuestionBankNotFoundException>(() =>
            _service.CreateQuestionAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task GetQuestionByIdAsync_Should_ReturnQuestion_When_Exists()
    {
        var questionId = Guid.NewGuid();
        var question = new Question
        {
            Id = questionId,
            Text = "Test question",
            Category = "Math",
            Difficulty = 1,
            CreatedAt = DateTime.UtcNow
        };

        _mockQuestionRepository.Setup(r => r.GetByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var result = await _service.GetQuestionByIdAsync(questionId, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(questionId);
        result.Text.Should().Be("Test question");
    }

    [Fact]
    public async Task GetQuestionByIdAsync_Should_ThrowQuestionNotFoundException_When_NotExists()
    {
        var questionId = Guid.NewGuid();
        _mockQuestionRepository.Setup(r => r.GetByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Question?)null);

        await Assert.ThrowsAsync<QuestionNotFoundException>(() =>
            _service.GetQuestionByIdAsync(questionId, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteQuestionAsync_Should_CallDelete_When_QuestionExists()
    {
        var questionId = Guid.NewGuid();
        var question = new Question
        {
            Id = questionId,
            Text = "Test question",
            Category = "Math",
            Difficulty = 1,
            CreatedAt = DateTime.UtcNow
        };

        _mockQuestionRepository.Setup(r => r.GetByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        _mockQuestionRepository.Setup(r => r.DeleteAsync(questionId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _service.DeleteQuestionAsync(questionId, CancellationToken.None);

        _mockQuestionRepository.Verify(r => r.DeleteAsync(questionId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteQuestionAsync_Should_ThrowQuestionNotFoundException_When_NotExists()
    {
        var questionId = Guid.NewGuid();
        _mockQuestionRepository.Setup(r => r.GetByIdAsync(questionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Question?)null);

        await Assert.ThrowsAsync<QuestionNotFoundException>(() =>
            _service.DeleteQuestionAsync(questionId, CancellationToken.None));
    }

    [Fact]
    public async Task GetQuestionsAsync_Should_ReturnFilteredQuestions()
    {
        var questions = new List<Question>
        {
            new() { Id = Guid.NewGuid(), Text = "Question 1", Category = "Math", Difficulty = 1, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Text = "Question 2", Category = "Math", Difficulty = 2, CreatedAt = DateTime.UtcNow }
        };

        _mockQuestionRepository.Setup(r => r.GetByFilterAsync("Math", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(questions);

        var result = await _service.GetQuestionsAsync("Math", null, null, CancellationToken.None);

        result.Should().NotBeNull();
        result.Questions.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }
}
