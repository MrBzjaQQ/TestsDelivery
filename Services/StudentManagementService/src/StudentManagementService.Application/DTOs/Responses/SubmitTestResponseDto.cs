namespace StudentManagementService.Application.DTOs.Responses;

public class SubmitTestResponseDto
{
    public short AttemptNumber { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; }
}
