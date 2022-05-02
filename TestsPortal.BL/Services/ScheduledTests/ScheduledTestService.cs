using TestsPortal.BL.Services.Candidates;
using AutoMapper;
using TestsPortal.BL.Services.Tests;
using TestsPortal.DAL.Repositories.ScheduledTests;
using TestsPortal.DAL.Models.ScheduledTests;
using ScheduledTestDomain = TestsPortal.Domain.ScheduledTests.ScheduledTest;
using ScheduledTestDAL = TestsPortal.DAL.Models.ScheduledTests.ScheduledTest;
using TestsPortal.DAL.Repositories.ScheduledTestInstances;
using TestsPortal.Domain.ScheduledTests;

namespace TestsPortal.BL.Services.ScheduledTests
{
    public class ScheduledTestService : IScheduledTestService
    {
        private readonly ICandidateService _candidateService;
        private readonly ITestsService _testsService;
        private readonly IScheduledTestsRepository _scheduledTestsRepository;
        private readonly IScheduledTestInstancesRepository _scheduledTestInstancesRepository;
        private readonly IMapper _mapper;

        public ScheduledTestService(
            ICandidateService candidateService,
            ITestsService testsService,
            IScheduledTestsRepository scheduledTestsRepository,
            IScheduledTestInstancesRepository scheduledTestInstancesRepository,
            IMapper mapper)
        {
            _candidateService = candidateService;
            _testsService = testsService;
            _scheduledTestsRepository = scheduledTestsRepository;
            _scheduledTestInstancesRepository = scheduledTestInstancesRepository;
            _mapper = mapper;
        }

        public ScheduledTestDomain ScheduleTest(ScheduledTestDomain scheduledTest)
        {
            var candidates = _candidateService.CreateCandidates(scheduledTest.Candidates);
            var test = _testsService.CreateTest(scheduledTest.Test);
            var dalScheduledTest = _mapper.Map<ScheduledTestDAL>(scheduledTest);
            _scheduledTestsRepository.Create(dalScheduledTest);
            var instances = MapTestInstances(dalScheduledTest.Id, scheduledTest, candidates);
            _scheduledTestInstancesRepository.Create(instances);

            var resultTest = _mapper.Map<ScheduledTestDomain>(dalScheduledTest);
            resultTest.Candidates = candidates;
            resultTest.Test = test;
            return resultTest;
        }

        private IEnumerable<ScheduledTestInstance> MapTestInstances(
            long testId,
            ScheduledTestDomain test,
            IEnumerable<Domain.Candidates.Candidate> candidates)
        {
            return candidates.Select(x => new ScheduledTestInstance
            {
                CandidateId = x.Id,
                ScheduledTestId = testId,
                Status = (short)TestStatus.NotStarted,
                Keycode = "ABCDEF",
                Pin = "ABCDEF",
                AdminPanelInstance = test.AdminPanelInstance
            });
        }
    }
}
