namespace NotificationService.Domain.Exceptions;

public class NotificationNotFoundException : Exception
{
    public Guid NotificationId { get; }

    public NotificationNotFoundException(Guid notificationId)
        : base($"Notification with id '{notificationId}' not found")
    {
        NotificationId = notificationId;
    }
}
