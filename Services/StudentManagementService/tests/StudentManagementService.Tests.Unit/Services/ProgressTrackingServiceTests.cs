using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Application.Services;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Domain.Exceptions;
using Xunit;

namespace StudentManagementService.Tests.Unit.Services;

public class ProgressTrackingServiceTests
{
    private readonly Mock<IStudentRepository> _mockStudentRepository;
    private readonly Mock<ITestAssignmentRepository> _mockAssignmentRepository;
    private readonly Mock<ITestProgressRepository> _mockProgressRepository;
    private readonly Mock<ILogger<ProgressTrackingService>> _mockLogger;
    private readonly ProgressTrackingService _service;

    public ProgressTrackingServiceTests()
    {
        _mockStudentRepository = new Mock<IStudentRepository>();
        _mockAssignmentRepository = new Mock<ITestAssignmentRepository>();
        _mockProgressRepository = new Mock<ITestProgressRepository>();
        _mockLogger = new Mock<ILogger<ProgressTrackingService>>();
        _service = new ProgressTrackingService(
            _mockStudentRepository.Object,
            _mockAssignmentRepository.Object,
            _mockProgressRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task GetStudentProgress_Should_ReturnReport()
    {
        var studentId = Guid.NewGuid();
        var student = new Student
        {
            Id = studentId,
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Status = StudentStatus.Active,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
        var assignments = new List<TestAssignment>
        {
            new() { Id = Guid.NewGuid(), StudentId = studentId, TestId = Guid.NewGuid(), Status = TestAssignmentStatus.Completed, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), StudentId = studentId, TestId = Guid.NewGuid(), Status = TestAssignmentStatus.InProgress, CreatedAt = DateTime.UtcNow }
        };
        var progressList = new List<TestProgress>
        {
            new() { Id = Guid.NewGuid(), StudentId = studentId, TestId = assignments[0].TestId, Score = 85, MaxScore = 100, IsPassed = true, AttemptNumber = 1, CreatedAt = DateTime.UtcNow }
        };

        _mockStudentRepository.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _mockAssignmentRepository.Setup(r => r.GetByStudentIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignments);
        _mockProgressRepository.Setup(r => r.GetByStudentIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(progressList);

        var result = await _service.GetStudentProgressAsync(studentId, CancellationToken.None);

        result.Should().NotBeNull();
        result.StudentId.Should().Be(studentId);
        result.TotalTests.Should().Be(2);
        result.CompletedTests.Should().Be(1);
        result.InProgressTests.Should().Be(1);
        result.AverageScore.Should().Be(85);
    }

    [Fact]
    public async Task GetStudentProgress_Should_ThrowNotFound_WhenStudentNotExists()
    {
        var studentId = Guid.NewGuid();
        _mockStudentRepository.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        var act = async () => await _service.GetStudentProgressAsync(studentId, CancellationToken.None);

        await act.Should().ThrowAsync<StudentNotFoundException>();
    }

    [Fact]
    public async Task GetTestResults_Should_ReturnResults()
    {
        var studentId = Guid.NewGuid();
        var testId = Guid.NewGuid();
        var student = new Student
        {
            Id = studentId,
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Status = StudentStatus.Active,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
        var progressList = new List<TestProgress>
        {
            new()
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                TestId = testId,
                Score = 85,
                MaxScore = 100,
                IsPassed = true,
                AttemptNumber = 1,
                SubmittedAt = DateTime.UtcNow,
                Answers = [new() { QuestionId = Guid.NewGuid(), IsCorrect = true, PointsEarned = 10 }],
                CreatedAt = DateTime.UtcNow
            }
        };

        _mockStudentRepository.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _mockProgressRepository.Setup(r => r.GetByStudentAndTestAsync(studentId, testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(progressList);

        var result = await _service.GetTestResultsAsync(studentId, testId, CancellationToken.None);

        result.Should().NotBeNull();
        result.TestId.Should().Be(testId);
        result.Attempts.Should().HaveCount(1);
        result.Attempts[0].Score.Should().Be(85);
        result.Attempts[0].IsPassed.Should().BeTrue();
    }

    [Fact]
    public async Task GetTestResults_Should_ThrowNotFound_WhenNoProgress()
    {
        var studentId = Guid.NewGuid();
        var testId = Guid.NewGuid();
        var student = new Student
        {
            Id = studentId,
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            Status = StudentStatus.Active,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _mockStudentRepository.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _mockProgressRepository.Setup(r => r.GetByStudentAndTestAsync(studentId, testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var act = async () => await _service.GetTestResultsAsync(studentId, testId, CancellationToken.None);

        await act.Should().ThrowAsync<TestAssignmentNotFoundException>();
    }

    [Fact]
    public async Task UpdateTestProgress_Should_UpdateScore()
    {
        var studentId = Guid.NewGuid();
        var testId = Guid.NewGuid();
        var progress = new TestProgress
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            TestId = testId,
            MaxScore = 100,
            AttemptNumber = 1,
            CreatedAt = DateTime.UtcNow
        };
        var assignment = new TestAssignment
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            TestId = testId,
            Status = TestAssignmentStatus.InProgress,
            CreatedAt = DateTime.UtcNow
        };

        _mockProgressRepository.Setup(r => r.GetLatestByStudentAndTestAsync(studentId, testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(progress);
        _mockAssignmentRepository.Setup(r => r.GetByStudentAndTestAsync(studentId, testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignment);

        await _service.UpdateTestProgressAsync(studentId, testId, 85, true, CancellationToken.None);

        _mockProgressRepository.Verify(r => r.UpdateAsync(It.Is<TestProgress>(p => p.Score == 85 && p.IsPassed), It.IsAny<CancellationToken>()), Times.Once);
        _mockAssignmentRepository.Verify(r => r.UpdateAsync(It.Is<TestAssignment>(a => a.Status == TestAssignmentStatus.Completed), It.IsAny<CancellationToken>()), Times.Once);
    }
}
