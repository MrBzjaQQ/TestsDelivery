using BffPortalService.WebApi.Factories.Contract;
using BffPortalService.WebApi.Wrappers.Contract;

namespace BffPortalService.WebApi.Factories.Contract;

public interface IActivityWrapperFactory
{
    IActivityWrapper GetActivity();
}
