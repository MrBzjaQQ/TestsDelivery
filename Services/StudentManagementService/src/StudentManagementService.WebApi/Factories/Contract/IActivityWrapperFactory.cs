using StudentManagementService.WebApi.Wrappers.Contract;

namespace StudentManagementService.WebApi.Factories.Contract;

public interface IActivityWrapperFactory
{
    IActivityWrapper GetActivity();
}
