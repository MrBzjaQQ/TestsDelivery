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

public class TestServiceTests
{
    private readonly Mock<ITestRepository> _mockTestRepository;
    private readonly Mock<IQuestionBankRepository> _mockQuestionBankRepository;
    private readonly Mock<IQuestionRepository> _mockQuestionRepository;
    private readonly Mock<ITestTemplateRepository> _mockTemplateRepository;
    private readonly Mock<ILogger<TestService>> _mockLogger;
    private readonly TestService _service;

    public TestServiceTests()
    {
        _mockTestRepository = new Mock<ITestRepository>();
        _mockQuestionBankRepository = new Mock<IQuestionBankRepository>();
        _mockQuestionRepository = new Mock<IQuestionRepository>();
        _mockTemplateRepository = new Mock<ITestTemplateRepository>();
        _mockLogger = new Mock<ILogger<TestService>>();
        _service = new TestService(
            _mockTestRepository.Object,
            _mockQuestionBankRepository.Object,
            _mockQuestionRepository.Object,
            _mockTemplateRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CreateTestAsync_Should_ReturnSuccess_When_Valid()
    {
        var request = new CreateTestRequest
        {
            Title = "Math Test",
            Description = "Test description",
            QuestionBankId = Guid.NewGuid(),
            DurationMinutes = 60,
            PassingScore = 70,
            MaxAttempts = 3
        };

        var bank = new QuestionBank { Id = request.QuestionBankId, Name = "Math Bank", OwnerId = Guid.NewGuid() };
        _mockQuestionBankRepository.Setup(r => r.GetByIdAsync(request.QuestionBankId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bank);

        _mockTestRepository.Setup(r => r.AddAsync(It.IsAny<Test>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateTestAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
        result.Status.Should().Be("Draft");
        _mockTestRepository.Verify(r => r.AddAsync(It.IsAny<Test>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateTestAsync_Should_ThrowQuestionBankNotFoundException_When_BankNotExists()
    {
        var request = new CreateTestRequest
        {
            Title = "Math Test",
            QuestionBankId = Guid.NewGuid(),
            DurationMinutes = 60,
            PassingScore = 70,
            MaxAttempts = 3
        };

        _mockQuestionBankRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((QuestionBank?)null);

        await Assert.ThrowsAsync<QuestionBankNotFoundException>(() =>
            _service.CreateTestAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task GetTestByIdAsync_Should_ReturnTest_When_Exists()
    {
        var testId = Guid.NewGuid();
        var test = new Test
        {
            Id = testId,
            Title = "Math Test",
            Description = "Test",
            QuestionBankId = Guid.NewGuid(),
            DurationMinutes = 60,
            PassingScore = 70,
            MaxAttempts = 3,
            Status = "Active",
            CreatedAt = DateTime.UtcNow
        };

        _mockTestRepository.Setup(r => r.GetByIdAsync(testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(test);

        var result = await _service.GetTestByIdAsync(testId, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(testId);
        result.Title.Should().Be("Math Test");
    }

    [Fact]
    public async Task GetTestByIdAsync_Should_ThrowTestNotFoundException_When_NotExists()
    {
        var testId = Guid.NewGuid();
        _mockTestRepository.Setup(r => r.GetByIdAsync(testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Test?)null);

        await Assert.ThrowsAsync<TestNotFoundException>(() =>
            _service.GetTestByIdAsync(testId, CancellationToken.None));
    }

    [Fact]
    public async Task CopyTestAsync_Should_CreateCopy_When_OriginalExists()
    {
        var originalId = Guid.NewGuid();
        var originalTest = new Test
        {
            Id = originalId,
            Title = "Math Test",
            Description = "Test",
            QuestionBankId = Guid.NewGuid(),
            DurationMinutes = 60,
            PassingScore = 70,
            MaxAttempts = 3,
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            TestQuestions = []
        };

        _mockTestRepository.Setup(r => r.GetByIdAsync(originalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(originalTest);

        _mockTestRepository.Setup(r => r.AddAsync(It.IsAny<Test>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CopyTestAsync(originalId, CancellationToken.None);

        result.Should().NotBeNull();
        result.Title.Should().Be("Math Test (Copy)");
        result.Status.Should().Be("Draft");
    }

    [Fact]
    public async Task GenerateTestFromBankAsync_Should_GenerateTest_When_EnoughQuestions()
    {
        var bankId = Guid.NewGuid();
        var request = new GenerateTestRequest
        {
            Title = "Generated Test",
            QuestionCount = 3
        };

        var bank = new QuestionBank { Id = bankId, Name = "Math Bank", OwnerId = Guid.NewGuid() };
        var questions = new List<Question>
        {
            new() { Id = Guid.NewGuid(), Text = "Q1", Category = "Math", Difficulty = 1, QuestionBankId = bankId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Text = "Q2", Category = "Math", Difficulty = 1, QuestionBankId = bankId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Text = "Q3", Category = "Math", Difficulty = 1, QuestionBankId = bankId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Text = "Q4", Category = "Math", Difficulty = 1, QuestionBankId = bankId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Text = "Q5", Category = "Math", Difficulty = 1, QuestionBankId = bankId, CreatedAt = DateTime.UtcNow }
        };

        _mockQuestionBankRepository.Setup(r => r.GetByIdAsync(bankId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bank);

        _mockQuestionRepository.Setup(r => r.GetByBankIdAsync(bankId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(questions);

        _mockTestRepository.Setup(r => r.AddAsync(It.IsAny<Test>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.GenerateTestFromBankAsync(bankId, request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Title.Should().Be("Generated Test");
        result.Questions.Should().HaveCount(3);
    }

    [Fact]
    public async Task GenerateTestFromBankAsync_Should_ThrowValidationException_When_NotEnoughQuestions()
    {
        var bankId = Guid.NewGuid();
        var request = new GenerateTestRequest
        {
            Title = "Generated Test",
            QuestionCount = 10
        };

        var bank = new QuestionBank { Id = bankId, Name = "Math Bank", OwnerId = Guid.NewGuid() };
        var questions = new List<Question>
        {
            new() { Id = Guid.NewGuid(), Text = "Q1", Category = "Math", Difficulty = 1, QuestionBankId = bankId, CreatedAt = DateTime.UtcNow }
        };

        _mockQuestionBankRepository.Setup(r => r.GetByIdAsync(bankId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bank);

        _mockQuestionRepository.Setup(r => r.GetByBankIdAsync(bankId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(questions);

        await Assert.ThrowsAsync<TestValidationException>(() =>
            _service.GenerateTestFromBankAsync(bankId, request, CancellationToken.None));
    }
}
