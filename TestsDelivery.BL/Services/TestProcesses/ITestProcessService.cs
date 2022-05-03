using TestsDelivery.Domain.AnsweredTests;

namespace TestsDelivery.BL.Services.TestProcesses
{
    public interface ITestProcessService
    {
        void FinishTest(AnsweredTest test);
    }
}
