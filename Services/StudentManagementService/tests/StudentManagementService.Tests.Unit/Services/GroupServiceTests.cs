using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Application.Services;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Domain.Exceptions;
using Xunit;

namespace StudentManagementService.Tests.Unit.Services;

public class GroupServiceTests
{
    private readonly Mock<IStudyGroupRepository> _mockGroupRepository;
    private readonly Mock<IStudentRepository> _mockStudentRepository;
    private readonly Mock<ITestAssignmentRepository> _mockAssignmentRepository;
    private readonly Mock<ITestProgressRepository> _mockProgressRepository;
    private readonly Mock<ILogger<GroupService>> _mockLogger;
    private readonly GroupService _service;

    public GroupServiceTests()
    {
        _mockGroupRepository = new Mock<IStudyGroupRepository>();
        _mockStudentRepository = new Mock<IStudentRepository>();
        _mockAssignmentRepository = new Mock<ITestAssignmentRepository>();
        _mockProgressRepository = new Mock<ITestProgressRepository>();
        _mockLogger = new Mock<ILogger<GroupService>>();
        _service = new GroupService(
            _mockGroupRepository.Object,
            _mockStudentRepository.Object,
            _mockAssignmentRepository.Object,
            _mockProgressRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task GetGroupStatistics_Should_ReturnStatistics()
    {
        var groupId = Guid.NewGuid();
        var group = new StudyGroup
        {
            Id = groupId,
            Name = "CS-2024",
            StartYear = 2024,
            EndYear = 2028,
            CreatedAt = DateTime.UtcNow
        };
        var students = new List<Student>
        {
            new() { Id = Guid.NewGuid(), GroupId = groupId, FirstName = "John", LastName = "Doe", Email = "john@test.com", Status = StudentStatus.Active, EnrollmentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), GroupId = groupId, FirstName = "Jane", LastName = "Smith", Email = "jane@test.com", Status = StudentStatus.Active, EnrollmentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow }
        };

        _mockGroupRepository.Setup(r => r.GetByIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(group);
        _mockGroupRepository.Setup(r => r.GetStudentsByGroupIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(students);
        _mockProgressRepository.Setup(r => r.GetByStudentIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var result = await _service.GetGroupStatisticsAsync(groupId, CancellationToken.None);

        result.Should().NotBeNull();
        result.GroupId.Should().Be(groupId);
        result.GroupName.Should().Be("CS-2024");
        result.TotalStudents.Should().Be(2);
        result.ActiveStudents.Should().Be(2);
    }

    [Fact]
    public async Task GetGroupStatistics_Should_ThrowNotFound_WhenGroupNotExists()
    {
        var groupId = Guid.NewGuid();
        _mockGroupRepository.Setup(r => r.GetByIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((StudyGroup?)null);

        var act = async () => await _service.GetGroupStatisticsAsync(groupId, CancellationToken.None);

        await act.Should().ThrowAsync<GroupNotFoundException>();
    }

    [Fact]
    public async Task GetGroupStudents_Should_ReturnStudents()
    {
        var groupId = Guid.NewGuid();
        var group = new StudyGroup
        {
            Id = groupId,
            Name = "CS-2024",
            StartYear = 2024,
            EndYear = 2028,
            CreatedAt = DateTime.UtcNow
        };
        var students = new List<Student>
        {
            new() { Id = Guid.NewGuid(), GroupId = groupId, FirstName = "John", LastName = "Doe", Email = "john@test.com", Status = StudentStatus.Active, EnrollmentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), GroupId = groupId, FirstName = "Jane", LastName = "Smith", Email = "jane@test.com", Status = StudentStatus.Active, EnrollmentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow }
        };

        _mockGroupRepository.Setup(r => r.GetByIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(group);
        _mockGroupRepository.Setup(r => r.GetStudentsByGroupIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(students);

        var result = await _service.GetGroupStudentsAsync(groupId, CancellationToken.None);

        result.Should().NotBeNull();
        result.Students.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetGroupStudents_Should_ThrowNotFound_WhenGroupNotExists()
    {
        var groupId = Guid.NewGuid();
        _mockGroupRepository.Setup(r => r.GetByIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((StudyGroup?)null);

        var act = async () => await _service.GetGroupStudentsAsync(groupId, CancellationToken.None);

        await act.Should().ThrowAsync<GroupNotFoundException>();
    }
}
