using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementService.Application.DTOs.Requests;
using StudentManagementService.Application.Services;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Infrastructure.Database.Repositories;
using StudentManagementService.Tests.Integration.TestInfrastructure;
using Xunit;

namespace StudentManagementService.Tests.Integration.Services;

[Collection(nameof(DatabaseCollection))]
public class TestAssignmentServiceIntegrationTests : DbTestsBase
{
    private readonly TestAssignmentService _testAssignmentService;
    private readonly StudentRepository _studentRepository;

    public TestAssignmentServiceIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _studentRepository = new StudentRepository(DbContext);
        var assignmentRepository = new TestAssignmentRepository(DbContext);
        var progressRepository = new TestProgressRepository(DbContext);
        var logger = new Mock<ILogger<TestAssignmentService>>().Object;
        _testAssignmentService = new TestAssignmentService(
            assignmentRepository,
            _studentRepository,
            progressRepository,
            logger);
    }

    [Fact]
    public async Task AssignTest_Should_PersistToDatabase()
    {
        var student = new Student
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Student",
            Email = "test@integration.com",
            Status = StudentStatus.Active,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await DbContext.Students.AddAsync(student);
        await DbContext.SaveChangesAsync();

        var testId = Guid.NewGuid();
        var request = new AssignTestRequest
        {
            Deadline = DateTime.UtcNow.AddDays(7),
            AttemptsAllowed = 3
        };

        var result = await _testAssignmentService.AssignTestAsync(student.Id, testId, request, CancellationToken.None);

        result.Should().NotBeNull();
        result.StudentId.Should().Be(student.Id);
        result.TestId.Should().Be(testId);

        var savedAssignment = await DbContext.TestAssignments
            .FirstOrDefaultAsync(a => a.StudentId == student.Id && a.TestId == testId);
        savedAssignment.Should().NotBeNull();
        savedAssignment!.AttemptsAllowed.Should().Be(3);
    }

    [Fact]
    public async Task SubmitTest_Should_PersistProgress()
    {
        var student = new Student
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            FirstName = "Submit",
            LastName = "Test",
            Email = "submit@integration.com",
            Status = StudentStatus.Active,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
        var testId = Guid.NewGuid();
        var assignment = new TestAssignment
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            TestId = testId,
            Status = TestAssignmentStatus.Assigned,
            AttemptsAllowed = 3,
            AttemptsUsed = 0,
            CreatedAt = DateTime.UtcNow
        };

        await DbContext.Students.AddAsync(student);
        await DbContext.TestAssignments.AddAsync(assignment);
        await DbContext.SaveChangesAsync();

        var request = new SubmitTestRequest
        {
            Answers =
            [
                new() { QuestionId = Guid.NewGuid(), SelectedOptionId = Guid.NewGuid() }
            ]
        };

        var result = await _testAssignmentService.SubmitTestAsync(student.Id, testId, request, CancellationToken.None);

        result.Should().NotBeNull();
        result.AttemptNumber.Should().Be(1);
        result.Status.Should().Be("Submitted");

        var progress = await DbContext.TestProgresses
            .FirstOrDefaultAsync(p => p.StudentId == student.Id && p.TestId == testId);
        progress.Should().NotBeNull();
        progress!.AttemptNumber.Should().Be(1);
    }

    [Fact]
    public async Task GetStudentTests_Should_ReturnFromDatabase()
    {
        var student = new Student
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            FirstName = "Get",
            LastName = "Tests",
            Email = "gettests@integration.com",
            Status = StudentStatus.Active,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
        var assignments = new List<TestAssignment>
        {
            new() { Id = Guid.NewGuid(), StudentId = student.Id, TestId = Guid.NewGuid(), Status = TestAssignmentStatus.Assigned, AttemptsAllowed = 3, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), StudentId = student.Id, TestId = Guid.NewGuid(), Status = TestAssignmentStatus.Completed, AttemptsAllowed = 3, CreatedAt = DateTime.UtcNow }
        };

        await DbContext.Students.AddAsync(student);
        await DbContext.TestAssignments.AddRangeAsync(assignments);
        await DbContext.SaveChangesAsync();

        var result = await _testAssignmentService.GetStudentTestsAsync(student.Id, CancellationToken.None);

        result.Assignments.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }
}
