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

public class StudentServiceTests
{
    private readonly Mock<IStudentRepository> _mockRepository;
    private readonly Mock<ILogger<StudentService>> _mockLogger;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _mockRepository = new Mock<IStudentRepository>();
        _mockLogger = new Mock<ILogger<StudentService>>();
        _service = new StudentService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task RegisterStudent_Should_ReturnSuccess()
    {
        var request = new RegisterStudentRequest
        {
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@student.com"
        };

        var result = await _service.RegisterStudentAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Email.Should().Be("john.doe@student.com");
        result.Status.Should().Be("Active");

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetStudentById_Should_ReturnStudent_WhenExists()
    {
        var studentId = Guid.NewGuid();
        var student = new Student
        {
            Id = studentId,
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@student.com",
            Status = StudentStatus.Active,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        var result = await _service.GetStudentByIdAsync(studentId, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(studentId);
        result.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task GetStudentById_Should_ThrowNotFoundException_WhenNotExists()
    {
        var studentId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        var act = async () => await _service.GetStudentByIdAsync(studentId, CancellationToken.None);

        await act.Should().ThrowAsync<StudentNotFoundException>();
    }

    [Fact]
    public async Task UpdateStudentProfile_Should_UpdateAndReturn()
    {
        var studentId = Guid.NewGuid();
        var student = new Student
        {
            Id = studentId,
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@student.com",
            Status = StudentStatus.Active,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        var request = new UpdateStudentProfileRequest
        {
            FirstName = "Jane",
            LastName = "Smith",
            PhoneNumber = "+1234567890"
        };

        var result = await _service.UpdateStudentProfileAsync(studentId, request, CancellationToken.None);

        result.FirstName.Should().Be("Jane");
        result.LastName.Should().Be("Smith");
        result.PhoneNumber.Should().Be("+1234567890");

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateStudentProfile_Should_ThrowNotFoundException_WhenNotExists()
    {
        var studentId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        var request = new UpdateStudentProfileRequest
        {
            FirstName = "Jane",
            LastName = "Smith"
        };

        var act = async () => await _service.UpdateStudentProfileAsync(studentId, request, CancellationToken.None);

        await act.Should().ThrowAsync<StudentNotFoundException>();
    }

    [Fact]
    public async Task GetStudents_Should_ReturnByGroupId()
    {
        var groupId = Guid.NewGuid();
        var students = new List<Student>
        {
            new() { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john@test.com", Status = StudentStatus.Active },
            new() { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Email = "jane@test.com", Status = StudentStatus.Active }
        };

        _mockRepository.Setup(r => r.GetByGroupIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(students);

        var result = await _service.GetStudentsAsync(groupId, null, CancellationToken.None);

        result.Students.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetStudents_Should_ReturnByStatus()
    {
        var students = new List<Student>
        {
            new() { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john@test.com", Status = StudentStatus.Active }
        };

        _mockRepository.Setup(r => r.GetByStatusAsync(StudentStatus.Active, It.IsAny<CancellationToken>()))
            .ReturnsAsync(students);

        var result = await _service.GetStudentsAsync(null, "active", CancellationToken.None);

        result.Students.Should().HaveCount(1);
        result.Students[0].Status.Should().Be("Active");
    }

    [Fact]
    public async Task GetStudents_Should_ReturnByGroupIdAndStatus()
    {
        var groupId = Guid.NewGuid();
        var students = new List<Student>
        {
            new() { Id = Guid.NewGuid(), GroupId = groupId, FirstName = "John", LastName = "Doe", Email = "john@test.com", Status = StudentStatus.Active }
        };

        _mockRepository.Setup(r => r.GetByGroupIdAndStatusAsync(groupId, StudentStatus.Active, It.IsAny<CancellationToken>()))
            .ReturnsAsync(students);

        var result = await _service.GetStudentsAsync(groupId, "active", CancellationToken.None);

        result.Students.Should().HaveCount(1);
    }
}
