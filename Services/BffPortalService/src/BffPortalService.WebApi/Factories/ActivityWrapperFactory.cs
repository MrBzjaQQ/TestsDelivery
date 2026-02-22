using BffPortalService.WebApi.Factories.Contract;
using BffPortalService.WebApi.Wrappers;
using BffPortalService.WebApi.Wrappers.Contract;

namespace BffPortalService.WebApi.Factories;

public class ActivityWrapperFactory : IActivityWrapperFactory
{
    public IActivityWrapper GetActivity()
    {
        return new ActivityWrapper();
    }
}
