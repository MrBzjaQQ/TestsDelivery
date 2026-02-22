namespace StudentManagementService.Application.DTOs.Responses;

public class ProgressReportDto
{
    public Guid StudentId { get; set; }

    public int TotalTests { get; set; }

    public int CompletedTests { get; set; }

    public int InProgressTests { get; set; }

    public int AssignedTests { get; set; }

    public double AverageScore { get; set; }

    public List<CompletedTestDto> CompletedTestsList { get; set; } = [];
}

public class CompletedTestDto
{
    public Guid TestId { get; set; }

    public string? TestTitle { get; set; }

    public short Score { get; set; }

    public short MaxScore { get; set; }

    public bool IsPassed { get; set; }

    public DateTime? CompletedAt { get; set; }
}
