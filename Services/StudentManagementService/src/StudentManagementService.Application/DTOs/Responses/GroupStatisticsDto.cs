namespace StudentManagementService.Application.DTOs.Responses;

public class GroupStatisticsDto
{
    public Guid GroupId { get; set; }

    public string? GroupName { get; set; }

    public int TotalStudents { get; set; }

    public int ActiveStudents { get; set; }

    public int CompletedTests { get; set; }

    public double AverageScore { get; set; }

    public List<TopPerformerDto> TopPerformers { get; set; } = [];

    public List<StudentTestCountDto> TestsCompletedByStudents { get; set; } = [];
}

public class TopPerformerDto
{
    public Guid StudentId { get; set; }

    public string? StudentName { get; set; }

    public double AverageScore { get; set; }
}

public class StudentTestCountDto
{
    public Guid StudentId { get; set; }

    public int TestCount { get; set; }

    public double AverageScore { get; set; }
}
