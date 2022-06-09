using TestsDelivery.UserModels.Questions.BaseQuestion;
using TestsDelivery.UserModels.TestProcess;

namespace TestsPortal.BL.Mediators.TestProcesses
{
    public interface ITestProcessMediator
    {
        IEnumerable<QuestionInListModel> GetQuestionsByTestId(long testId);

        QuestionReadModel GetQuestionById(long questionId);

        void StartTest(StartTestModel model);

        Task FinishTest(long testId);
    }
}
