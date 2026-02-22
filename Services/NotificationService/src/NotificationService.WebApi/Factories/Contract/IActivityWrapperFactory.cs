using NotificationService.WebApi.Wrappers.Contract;

namespace NotificationService.WebApi.Factories.Contract;

public interface IActivityWrapperFactory
{
    IActivityWrapper GetActivity();
}
