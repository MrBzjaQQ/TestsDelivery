using TestsPortal.BL.Exceptions;
using TestsPortal.BL.FilterBuilders;
using TestsPortal.DAL.Repositories.ScheduledTestInstances;
using TestsPortal.DAL.Repositories.ScheduledTests;
using TestsPortal.Domain.ScheduledTests;
using TestsPortal.Domain.TestProcesses;

namespace TestsPortal.BL.Services.TestProcesses
{
    public class TestProcessService : ITestProcessService
    {
        private readonly IScheduledTestInstancesRepository _instancesRepository;

        public TestProcessService(IScheduledTestInstancesRepository instancesRepository)
        {
            _instancesRepository = instancesRepository;
        }

        public void FinishTest(long testId)
        {
            var test = _instancesRepository.GetById(testId);
            test.Status = (short)TestStatus.Completed;
            _instancesRepository.Update(test);
            // TODO: change test status in AdminPanel
        }

        public void StartTest(TestCredentials credentials)
        {
            var filter = new ScheduledTestsInstancesFilterBuilder()
                .ByTestId(credentials.TestId)
                .ByCandidateId(credentials.CandidateId)
                .ByKeycode(credentials.Keycode)
                .ByPin(credentials.Pin)
                .Build();

            var tests = _instancesRepository.GetByFilter(filter);

            if (tests.Count == 0)
                throw new CandidateAuthenticationException();

            var test = tests.Single();
            test.Status = (short)TestStatus.InProgress;
            _instancesRepository.Update(test);
            // TODO: change test status in AdminPanel
        }
    }
}
