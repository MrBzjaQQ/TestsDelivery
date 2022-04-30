using TestsPortal.BL.Exceptions;
using TestsPortal.BL.FilterBuilders;
using TestsPortal.DAL.Repositories.ScheduledTests;
using TestsPortal.Domain.ScheduledTests;
using TestsPortal.Domain.TestProcesses;

namespace TestsPortal.BL.Services.TestProcesses
{
    public class TestProcessService : ITestProcessService
    {
        private readonly IScheduledTestsRepository _scheduledTestsRepository;

        public TestProcessService(IScheduledTestsRepository scheduledTestsRepository)
        {
            _scheduledTestsRepository = scheduledTestsRepository;
        }

        public void FinishTest(long testId)
        {
            var test = _scheduledTestsRepository.GetById(testId);
            test.Status = (short)TestStatus.Completed;
            _scheduledTestsRepository.Update(test);
            // TODO: change test status in AdminPanel
        }

        public void StartTest(TestCredentials credentials)
        {
            var filter = new ScheduledTestsFilterBuilder()
                .ByTestId(credentials.TestId)
                .ByKeycode(credentials.Keycode)
                .ByPin(credentials.Pin)
                .Build();

            var tests = _scheduledTestsRepository.GetByFilter(filter);

            if (tests.Count == 0)
                throw new CandidateAuthenticationException();

            var test = tests.Single();
            test.Status = (short)TestStatus.InProgress;
            _scheduledTestsRepository.Update(test);
            // TODO: change test status in AdminPanel
        }
    }
}
