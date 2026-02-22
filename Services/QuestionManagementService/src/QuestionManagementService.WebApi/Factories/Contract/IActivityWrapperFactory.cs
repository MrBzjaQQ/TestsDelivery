using QuestionManagementService.WebApi.Wrappers.Contract;

namespace QuestionManagementService.WebApi.Factories.Contract;

public interface IActivityWrapperFactory
{
    IActivityWrapper GetActivity();
}
