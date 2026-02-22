using NotificationService.WebApi.Factories.Contract;
using NotificationService.WebApi.Wrappers;
using NotificationService.WebApi.Wrappers.Contract;

namespace NotificationService.WebApi.Factories;

public class ActivityWrapperFactory : IActivityWrapperFactory
{
    public IActivityWrapper GetActivity()
    {
        return new ActivityWrapper();
    }
}
