using StudentManagementService.WebApi.Factories.Contract;
using StudentManagementService.WebApi.Wrappers;
using StudentManagementService.WebApi.Wrappers.Contract;

namespace StudentManagementService.WebApi.Factories;

public class ActivityWrapperFactory : IActivityWrapperFactory
{
    public IActivityWrapper GetActivity()
    {
        return new ActivityWrapper();
    }
}
