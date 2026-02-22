using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using StudentManagementService.Application.Contracts;
using StudentManagementService.Application.DTOs.Requests;
using StudentManagementService.Application.DTOs.Responses;
using StudentManagementService.Application.Infrastructure.Database.Contract;
using StudentManagementService.Domain.Entities;
using StudentManagementService.Domain.Exceptions;

namespace StudentManagementService.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IStudentRepository studentRepository, ILogger<StudentService> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<StudentDto> RegisterStudentAsync(RegisterStudentRequest request, CancellationToken ct)
    {
        Guard.IsNotNull(request);

        var student = new Student
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            GroupId = request.GroupId,
            Status = StudentStatus.Active,
            EnrollmentDate = request.EnrollmentDate ?? DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await _studentRepository.AddAsync(student, ct);

        _logger.LogInformation("Student registered with id {StudentId}", student.Id);

        return MapToDto(student);
    }

    public async Task<StudentDto> GetStudentByIdAsync(Guid id, CancellationToken ct)
    {
        var student = await _studentRepository.GetByIdAsync(id, ct);

        if (student == null)
        {
            throw new StudentNotFoundException(id);
        }

        return MapToDto(student);
    }

    public async Task<StudentDto> UpdateStudentProfileAsync(Guid id, UpdateStudentProfileRequest request, CancellationToken ct)
    {
        Guard.IsNotNull(request);

        var student = await _studentRepository.GetByIdAsync(id, ct);

        if (student == null)
        {
            throw new StudentNotFoundException(id);
        }

        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.PhoneNumber = request.PhoneNumber;

        if (request.Profile != null)
        {
            student.Profile = new StudentProfile
            {
                AvatarUrl = request.Profile.AvatarUrl,
                Bio = request.Profile.Bio
            };
        }

        await _studentRepository.UpdateAsync(student, ct);

        _logger.LogInformation("Student profile updated for id {StudentId}", student.Id);

        return MapToDto(student);
    }

    public async Task<StudentListDto> GetStudentsAsync(Guid? groupId, string? status, CancellationToken ct)
    {
        List<Student> students;

        if (groupId.HasValue && !string.IsNullOrEmpty(status))
        {
            var studentStatus = ParseStatus(status);
            students = await _studentRepository.GetByGroupIdAndStatusAsync(groupId.Value, studentStatus, ct);
        }
        else if (groupId.HasValue)
        {
            students = await _studentRepository.GetByGroupIdAsync(groupId.Value, ct);
        }
        else if (!string.IsNullOrEmpty(status))
        {
            var studentStatus = ParseStatus(status);
            students = await _studentRepository.GetByStatusAsync(studentStatus, ct);
        }
        else
        {
            throw new ArgumentException("At least one filter parameter must be provided");
        }

        return new StudentListDto
        {
            Students = students.Select(MapToDto).ToList(),
            TotalCount = students.Count
        };
    }

    private static StudentStatus ParseStatus(string status)
    {
        return status.ToLowerInvariant() switch
        {
            "active" => StudentStatus.Active,
            "inactive" => StudentStatus.Inactive,
            "graduated" => StudentStatus.Graduated,
            _ => throw new ArgumentException($"Invalid status value: {status}")
        };
    }

    private static StudentDto MapToDto(Student student)
    {
        return new StudentDto
        {
            Id = student.Id,
            UserId = student.UserId,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            GroupId = student.GroupId,
            PhoneNumber = student.PhoneNumber,
            Profile = student.Profile != null
                ? new StudentProfileDto { AvatarUrl = student.Profile.AvatarUrl, Bio = student.Profile.Bio }
                : null,
            Status = student.Status.ToString(),
            EnrollmentDate = student.EnrollmentDate
        };
    }
}
