using TestsPortal.Domain.TestProcesses;

namespace TestsPortal.BL.Services.TestProcesses
{
    public interface ITestProcessService
    {
        void StartTest(TestCredentials credentials);

        void FinishTest(long testId);
    }
}
