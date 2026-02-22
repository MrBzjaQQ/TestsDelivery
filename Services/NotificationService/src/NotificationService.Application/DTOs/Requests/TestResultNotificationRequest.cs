namespace NotificationService.Application.DTOs.Requests;

public class TestResultNotificationRequest
{
    public string StudentEmail { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public string TestTitle { get; set; } = string.Empty;

    public int Score { get; set; }

    public int MaxScore { get; set; }

    public double Percentage { get; set; }

    public bool IsPassed { get; set; }
}
