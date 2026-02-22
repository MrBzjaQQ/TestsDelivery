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

public class QuestionBankServiceTests
{
    private readonly Mock<IQuestionBankRepository> _mockQuestionBankRepository;
    private readonly Mock<IQuestionRepository> _mockQuestionRepository;
    private readonly Mock<ILogger<QuestionBankService>> _mockLogger;
    private readonly QuestionBankService _service;

    public QuestionBankServiceTests()
    {
        _mockQuestionBankRepository = new Mock<IQuestionBankRepository>();
        _mockQuestionRepository = new Mock<IQuestionRepository>();
        _mockLogger = new Mock<ILogger<QuestionBankService>>();
        _service = new QuestionBankService(
            _mockQuestionBankRepository.Object,
            _mockQuestionRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CreateQuestionBankAsync_Should_ReturnSuccess_When_Valid()
    {
        var request = new CreateQuestionBankRequest
        {
            Name = "Math Bank",
            Description = "Math questions",
            OwnerId = Guid.NewGuid()
        };

        _mockQuestionBankRepository.Setup(r => r.AddAsync(It.IsAny<QuestionBank>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateQuestionBankAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Description.Should().Be(request.Description);
        _mockQuestionBankRepository.Verify(r => r.AddAsync(It.IsAny<QuestionBank>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetQuestionBankByIdAsync_Should_ReturnBank_When_Exists()
    {
        var bankId = Guid.NewGuid();
        var bank = new QuestionBank
        {
            Id = bankId,
            Name = "Math Bank",
            Description = "Math questions",
            OwnerId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        _mockQuestionBankRepository.Setup(r => r.GetByIdAsync(bankId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bank);

        _mockQuestionBankRepository.Setup(r => r.GetQuestionsCountAsync(bankId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(5);

        var result = await _service.GetQuestionBankByIdAsync(bankId, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(bankId);
        result.Name.Should().Be("Math Bank");
        result.QuestionsCount.Should().Be(5);
    }

    [Fact]
    public async Task GetQuestionBankByIdAsync_Should_ThrowNotFoundException_When_NotExists()
    {
        var bankId = Guid.NewGuid();
        _mockQuestionBankRepository.Setup(r => r.GetByIdAsync(bankId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((QuestionBank?)null);

        await Assert.ThrowsAsync<QuestionBankNotFoundException>(() =>
            _service.GetQuestionBankByIdAsync(bankId, CancellationToken.None));
    }

    [Fact]
    public async Task GetQuestionsByBankIdAsync_Should_ReturnQuestions_When_BankExists()
    {
        var bankId = Guid.NewGuid();
        var bank = new QuestionBank { Id = bankId, Name = "Math Bank", OwnerId = Guid.NewGuid() };
        var questions = new List<Question>
        {
            new() { Id = Guid.NewGuid(), Text = "Q1", Category = "Math", Difficulty = 1, QuestionBankId = bankId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Text = "Q2", Category = "Math", Difficulty = 2, QuestionBankId = bankId, CreatedAt = DateTime.UtcNow }
        };

        _mockQuestionBankRepository.Setup(r => r.GetByIdAsync(bankId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bank);

        _mockQuestionRepository.Setup(r => r.GetByBankIdAsync(bankId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(questions);

        var result = await _service.GetQuestionsByBankIdAsync(bankId, CancellationToken.None);

        result.Should().NotBeNull();
        result.Questions.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }
}
