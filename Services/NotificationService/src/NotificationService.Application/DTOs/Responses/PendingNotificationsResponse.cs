namespace NotificationService.Application.DTOs.Responses;

public record PendingNotificationsResponse(
    List<NotificationStatusDto> Notifications,
    int TotalCount);
