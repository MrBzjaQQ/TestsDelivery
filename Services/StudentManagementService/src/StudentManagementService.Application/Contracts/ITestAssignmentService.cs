using StudentManagementService.Application.DTOs.Requests;
using StudentManagementService.Application.DTOs.Responses;
using StudentManagementService.Domain.Entities;

namespace StudentManagementService.Application.Contracts;

public interface ITestAssignmentService
{
    Task<TestAssignmentDto> AssignTestAsync(Guid studentId, Guid testId, AssignTestRequest request, CancellationToken ct);

    Task<TestAssignmentListDto> GetStudentTestsAsync(Guid studentId, CancellationToken ct);

    Task<TestAssignmentListDto> GetActiveTestsAsync(Guid studentId, CancellationToken ct);

    Task<SubmitTestResponseDto> SubmitTestAsync(Guid studentId, Guid testId, SubmitTestRequest request, CancellationToken ct);
}
