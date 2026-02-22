using TestCheckingService.WebApi.Wrappers.Contract;

namespace TestCheckingService.WebApi.Factories.Contract;

public interface IActivityWrapperFactory
{
    IActivityWrapper? GetActivity();
}
