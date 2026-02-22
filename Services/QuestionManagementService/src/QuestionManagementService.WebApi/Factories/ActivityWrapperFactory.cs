using QuestionManagementService.WebApi.Factories.Contract;
using QuestionManagementService.WebApi.Wrappers;
using QuestionManagementService.WebApi.Wrappers.Contract;

namespace QuestionManagementService.WebApi.Factories;

public class ActivityWrapperFactory : IActivityWrapperFactory
{
    public IActivityWrapper GetActivity()
    {
        return new ActivityWrapper();
    }
}
