using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementService.Application.DTOs.Requests;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Application.Services;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Domain.Exceptions;
using Xunit;

namespace StudentManagementService.Tests.Unit.Services;

public class TestAssignmentServiceTests
{
    private readonly Mock<ITestAssignmentRepository> _mockAssignmentRepository;
    private readonly Mock<IStudentRepository> _mockStudentRepository;
    private readonly Mock<ITestProgressRepository> _mockProgressRepository;
    private readonly Mock<ILogger<TestAssignmentService>> _mockLogger;
    private readonly TestAssignmentService _service;

    public TestAssignmentServiceTests()
    {
        _mockAssignmentRepository = new Mock<ITestAssignmentRepository>();
        _mockStudentRepository = new Mock<IStudentRepository>();
        _mockProgressRepository = new Mock<ITestProgressRepository>();
        _mockLogger = new Mock<ILogger<TestAssignmentService>>();
        _service = new TestAssignmentService(
            _mockAssignmentRepository.Object,
            _mockStudentRepository.Object,
            _mockProgressRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task AssignTest_Should_ReturnSuccess()
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
        _mockAssignmentRepository.Setup(r => r.GetByStudentAndTestAsync(studentId, testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestAssignment?)null);

        var request = new AssignTestRequest
        {
            Deadline = DateTime.UtcNow.AddDays(7),
            AttemptsAllowed = 3
        };

        var result = await _service.AssignTestAsync(studentId, testId, request, CancellationToken.None);

        result.Should().NotBeNull();
        result.StudentId.Should().Be(studentId);
        result.TestId.Should().Be(testId);
        result.Status.Should().Be("Assigned");
        result.AttemptsAllowed.Should().Be(3);

        _mockAssignmentRepository.Verify(r => r.AddAsync(It.IsAny<TestAssignment>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AssignTest_Should_ThrowStudentNotFound_WhenStudentNotExists()
    {
        var studentId = Guid.NewGuid();
        var testId = Guid.NewGuid();

        _mockStudentRepository.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        var request = new AssignTestRequest();

        var act = async () => await _service.AssignTestAsync(studentId, testId, request, CancellationToken.None);

        await act.Should().ThrowAsync<StudentNotFoundException>();
    }

    [Fact]
    public async Task AssignTest_Should_ThrowAlreadyEnrolled_WhenAlreadyAssigned()
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
        var existingAssignment = new TestAssignment
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            TestId = testId,
            Status = TestAssignmentStatus.Assigned,
            CreatedAt = DateTime.UtcNow
        };

        _mockStudentRepository.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _mockAssignmentRepository.Setup(r => r.GetByStudentAndTestAsync(studentId, testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAssignment);

        var request = new AssignTestRequest();

        var act = async () => await _service.AssignTestAsync(studentId, testId, request, CancellationToken.None);

        await act.Should().ThrowAsync<StudentAlreadyEnrolledException>();
    }

    [Fact]
    public async Task GetStudentTests_Should_ReturnAssignments()
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
            new() { Id = Guid.NewGuid(), StudentId = studentId, TestId = Guid.NewGuid(), Status = TestAssignmentStatus.Assigned, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), StudentId = studentId, TestId = Guid.NewGuid(), Status = TestAssignmentStatus.InProgress, CreatedAt = DateTime.UtcNow }
        };

        _mockStudentRepository.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _mockAssignmentRepository.Setup(r => r.GetByStudentIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignments);

        var result = await _service.GetStudentTestsAsync(studentId, CancellationToken.None);

        result.Assignments.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetActiveTests_Should_ReturnOnlyActive()
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
            new() { Id = Guid.NewGuid(), StudentId = studentId, TestId = Guid.NewGuid(), Status = TestAssignmentStatus.Assigned, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), StudentId = studentId, TestId = Guid.NewGuid(), Status = TestAssignmentStatus.InProgress, CreatedAt = DateTime.UtcNow }
        };

        _mockStudentRepository.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _mockAssignmentRepository.Setup(r => r.GetActiveByStudentIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignments);

        var result = await _service.GetActiveTestsAsync(studentId, CancellationToken.None);

        result.Assignments.Should().HaveCount(2);
    }

    [Fact]
    public async Task SubmitTest_Should_ReturnSuccess()
    {
        var studentId = Guid.NewGuid();
        var testId = Guid.NewGuid();
        var assignment = new TestAssignment
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            TestId = testId,
            Status = TestAssignmentStatus.Assigned,
            AttemptsAllowed = 3,
            AttemptsUsed = 0,
            CreatedAt = DateTime.UtcNow
        };

        _mockAssignmentRepository.Setup(r => r.GetByStudentAndTestAsync(studentId, testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignment);
        _mockProgressRepository.Setup(r => r.GetLatestByStudentAndTestAsync(studentId, testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestProgress?)null);

        var request = new SubmitTestRequest
        {
            Answers =
            [
                new() { QuestionId = Guid.NewGuid(), SelectedOptionId = Guid.NewGuid() }
            ]
        };

        var result = await _service.SubmitTestAsync(studentId, testId, request, CancellationToken.None);

        result.Should().NotBeNull();
        result.AttemptNumber.Should().Be(1);
        result.Status.Should().Be("Submitted");

        _mockProgressRepository.Verify(r => r.AddAsync(It.IsAny<TestProgress>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockAssignmentRepository.Verify(r => r.UpdateAsync(It.IsAny<TestAssignment>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SubmitTest_Should_ThrowNotEligible_WhenDeadlinePassed()
    {
        var studentId = Guid.NewGuid();
        var testId = Guid.NewGuid();
        var assignment = new TestAssignment
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            TestId = testId,
            Status = TestAssignmentStatus.Assigned,
            Deadline = DateTime.UtcNow.AddDays(-1),
            AttemptsAllowed = 3,
            AttemptsUsed = 0,
            CreatedAt = DateTime.UtcNow
        };

        _mockAssignmentRepository.Setup(r => r.GetByStudentAndTestAsync(studentId, testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignment);

        var request = new SubmitTestRequest { Answers = [] };

        var act = async () => await _service.SubmitTestAsync(studentId, testId, request, CancellationToken.None);

        await act.Should().ThrowAsync<TestNotEligibleException>();
    }

    [Fact]
    public async Task SubmitTest_Should_ThrowNotEligible_WhenMaxAttemptsReached()
    {
        var studentId = Guid.NewGuid();
        var testId = Guid.NewGuid();
        var assignment = new TestAssignment
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            TestId = testId,
            Status = TestAssignmentStatus.Assigned,
            Deadline = DateTime.UtcNow.AddDays(1),
            AttemptsAllowed = 3,
            AttemptsUsed = 3,
            CreatedAt = DateTime.UtcNow
        };

        _mockAssignmentRepository.Setup(r => r.GetByStudentAndTestAsync(studentId, testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignment);

        var request = new SubmitTestRequest { Answers = [] };

        var act = async () => await _service.SubmitTestAsync(studentId, testId, request, CancellationToken.None);

        await act.Should().ThrowAsync<TestNotEligibleException>();
    }

    [Fact]
    public async Task SubmitTest_Should_ThrowNotFound_WhenAssignmentNotExists()
    {
        var studentId = Guid.NewGuid();
        var testId = Guid.NewGuid();

        _mockAssignmentRepository.Setup(r => r.GetByStudentAndTestAsync(studentId, testId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TestAssignment?)null);

        var request = new SubmitTestRequest { Answers = [] };

        var act = async () => await _service.SubmitTestAsync(studentId, testId, request, CancellationToken.None);

        await act.Should().ThrowAsync<TestAssignmentNotFoundException>();
    }
}
