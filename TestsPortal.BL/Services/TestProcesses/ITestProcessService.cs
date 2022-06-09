using TestsPortal.Domain.AnsweredTests;
using TestsPortal.Domain.TestProcesses;

namespace TestsPortal.BL.Services.TestProcesses
{
    public interface ITestProcessService
    {
        void StartTest(TestCredentials credentials);

        string GetAdminInstanceForTest(long testId);

        AnsweredTest FinishTest(long testId);
    }
}
