using Microsoft.Extensions.Logging;
using StudentManagementService.Application.Contracts;
using StudentManagementService.Application.DTOs.Responses;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Domain.Exceptions;

namespace StudentManagementService.Application.Services;

public class GroupService : IGroupService
{
    private readonly IStudyGroupRepository _groupRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITestAssignmentRepository _assignmentRepository;
    private readonly ITestProgressRepository _progressRepository;
    private readonly ILogger<GroupService> _logger;

    public GroupService(
        IStudyGroupRepository groupRepository,
        IStudentRepository studentRepository,
        ITestAssignmentRepository assignmentRepository,
        ITestProgressRepository progressRepository,
        ILogger<GroupService> logger)
    {
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
        _assignmentRepository = assignmentRepository;
        _progressRepository = progressRepository;
        _logger = logger;
    }

    public async Task<GroupStatisticsDto> GetGroupStatisticsAsync(Guid groupId, CancellationToken ct)
    {
        var group = await _groupRepository.GetByIdAsync(groupId, ct);
        if (group == null)
        {
            throw new GroupNotFoundException(groupId);
        }

        var students = await _groupRepository.GetStudentsByGroupIdAsync(groupId, ct);
        var activeStudents = students.Where(s => s.Status == StudentStatus.Active).ToList();

        var allProgress = new List<TestProgress>();
        foreach (var student in activeStudents)
        {
            var progress = await _progressRepository.GetByStudentIdAsync(student.Id, ct);
            allProgress.AddRange(progress);
        }

        var completedProgress = allProgress.Where(p => p.Score.HasValue).ToList();

        var studentScores = activeStudents.Select(student =>
        {
            var studentProgress = allProgress.Where(p => p.StudentId == student.Id && p.Score.HasValue).ToList();
            return new
            {
                StudentId = student.Id,
                StudentName = $"{student.FirstName} {student.LastName}",
                AverageScore = studentProgress.Count != 0 ? studentProgress.Average(p => p.Score!.Value) : 0,
                TestCount = studentProgress.Count
            };
        }).OrderByDescending(s => s.AverageScore).ToList();

        return new GroupStatisticsDto
        {
            GroupId = groupId,
            GroupName = group.Name,
            TotalStudents = students.Count,
            ActiveStudents = activeStudents.Count,
            CompletedTests = completedProgress.Count,
            AverageScore = completedProgress.Count != 0 ? completedProgress.Average(p => p.Score!.Value) : 0,
            TopPerformers = studentScores.Take(5).Select(s => new TopPerformerDto
            {
                StudentId = s.StudentId,
                StudentName = s.StudentName,
                AverageScore = s.AverageScore
            }).ToList(),
            TestsCompletedByStudents = studentScores.Select(s => new StudentTestCountDto
            {
                StudentId = s.StudentId,
                TestCount = s.TestCount,
                AverageScore = s.AverageScore
            }).ToList()
        };
    }

    public async Task<StudentListDto> GetGroupStudentsAsync(Guid groupId, CancellationToken ct)
    {
        var group = await _groupRepository.GetByIdAsync(groupId, ct);
        if (group == null)
        {
            throw new GroupNotFoundException(groupId);
        }

        var students = await _groupRepository.GetStudentsByGroupIdAsync(groupId, ct);

        return new StudentListDto
        {
            Students = students.Select(s => new StudentDto
            {
                Id = s.Id,
                UserId = s.UserId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                GroupId = s.GroupId,
                PhoneNumber = s.PhoneNumber,
                Profile = s.Profile != null
                    ? new StudentProfileDto { AvatarUrl = s.Profile.AvatarUrl, Bio = s.Profile.Bio }
                    : null,
                Status = s.Status.ToString(),
                EnrollmentDate = s.EnrollmentDate
            }).ToList(),
            TotalCount = students.Count
        };
    }

    public async Task<StudyGroup?> GetGroupByIdAsync(Guid groupId, CancellationToken ct)
    {
        return await _groupRepository.GetByIdAsync(groupId, ct);
    }
}
