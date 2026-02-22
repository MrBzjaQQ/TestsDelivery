using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementService.Application.DTOs.Requests;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Application.Services;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Infrastructure.Database.Repositories;
using StudentManagementService.Tests.Integration.TestInfrastructure;
using Xunit;

namespace StudentManagementService.Tests.Integration.Services;

[Collection(nameof(DatabaseCollection))]
public class StudentServiceIntegrationTests : DbTestsBase
{
    private readonly StudentService _studentService;

    public StudentServiceIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        var studentRepository = new StudentRepository(DbContext);
        var logger = new Mock<ILogger<StudentService>>().Object;
        _studentService = new StudentService(studentRepository, logger);
    }

    [Fact]
    public async Task RegisterStudent_Should_PersistToDatabase()
    {
        var request = new RegisterStudentRequest
        {
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@integration.com"
        };

        var result = await _studentService.RegisterStudentAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);

        var savedStudent = await DbContext.Students.FirstOrDefaultAsync(s => s.Id == result.Id);
        savedStudent.Should().NotBeNull();
        savedStudent!.FirstName.Should().Be("John");
        savedStudent.LastName.Should().Be("Doe");
        savedStudent.Email.Should().Be("john.doe@integration.com");
    }

    [Fact]
    public async Task GetStudentById_Should_ReturnFromDatabase()
    {
        var student = new Student
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@integration.com",
            Status = StudentStatus.Active,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await DbContext.Students.AddAsync(student);
        await DbContext.SaveChangesAsync();

        var result = await _studentService.GetStudentByIdAsync(student.Id, CancellationToken.None);

        result.Should().NotBeNull();
        result.FirstName.Should().Be("Jane");
        result.LastName.Should().Be("Smith");
    }

    [Fact]
    public async Task UpdateStudentProfile_Should_UpdateDatabase()
    {
        var student = new Student
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            FirstName = "Original",
            LastName = "Name",
            Email = "original@integration.com",
            Status = StudentStatus.Active,
            EnrollmentDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await DbContext.Students.AddAsync(student);
        await DbContext.SaveChangesAsync();

        var request = new UpdateStudentProfileRequest
        {
            FirstName = "Updated",
            LastName = "Profile",
            PhoneNumber = "+1234567890"
        };

        var result = await _studentService.UpdateStudentProfileAsync(student.Id, request, CancellationToken.None);

        result.FirstName.Should().Be("Updated");
        result.LastName.Should().Be("Profile");

        var updatedStudent = await DbContext.Students.FirstOrDefaultAsync(s => s.Id == student.Id);
        updatedStudent!.FirstName.Should().Be("Updated");
        updatedStudent.LastName.Should().Be("Profile");
        updatedStudent.PhoneNumber.Should().Be("+1234567890");
    }
}
