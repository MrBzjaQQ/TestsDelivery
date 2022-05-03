using TestsDelivery.UserModels.AnsweredTests;

namespace TestsDelivery.BL.Mediators.TestProcesses
{
    public interface ITestProcessMediator
    {
        void FinishTest(AnsweredTestCreateModel answeredTest);

        void StartTest(long testId);
    }
}
