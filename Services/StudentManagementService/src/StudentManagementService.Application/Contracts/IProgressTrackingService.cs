using StudentManagementService.Application.DTOs.Responses;

namespace StudentManagementService.Application.Contracts;

public interface IProgressTrackingService
{
    Task<ProgressReportDto> GetStudentProgressAsync(Guid studentId, CancellationToken ct);

    Task<TestResultDto> GetTestResultsAsync(Guid studentId, Guid testId, CancellationToken ct);

    Task UpdateTestProgressAsync(Guid studentId, Guid testId, short score, bool isPassed, CancellationToken ct);
}
