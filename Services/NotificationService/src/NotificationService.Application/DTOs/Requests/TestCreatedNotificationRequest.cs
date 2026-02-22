namespace NotificationService.Application.DTOs.Requests;

public class TestCreatedNotificationRequest
{
    public Guid StudentId { get; set; }

    public string StudentEmail { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;
}
