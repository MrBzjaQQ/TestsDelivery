using TestCheckingService.WebApi.Factories.Contract;
using TestCheckingService.WebApi.Wrappers;
using TestCheckingService.WebApi.Wrappers.Contract;

namespace TestCheckingService.WebApi.Factories;

public class ActivityWrapperFactory : IActivityWrapperFactory
{
    public IActivityWrapper? GetActivity()
    {
        return new ActivityWrapper();
    }
}
