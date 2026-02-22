using NotificationService.Domain.Entities;
using NotificationService.Domain.ValueObjects;

namespace NotificationService.Application.Specifications.NotificationSpecs;

public class PendingNotificationSpecification
{
    public bool IsSatisfiedBy(Notification notification)
    {
        return notification.Status == NotificationStatus.Pending ||
               notification.Status == NotificationStatus.Retrying;
    }
}
