using FileStorageService.WebApi.Factories.Contract;
using FileStorageService.WebApi.Wrappers;
using FileStorageService.WebApi.Wrappers.Contract;

namespace FileStorageService.WebApi.Factories;

public class ActivityWrapperFactory : IActivityWrapperFactory
{
    public IActivityWrapper GetActivity()
    {
        return new ActivityWrapper();
    }
}
