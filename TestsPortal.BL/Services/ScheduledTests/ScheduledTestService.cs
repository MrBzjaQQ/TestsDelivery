using TestsPortal.BL.Services.Candidates;
using AutoMapper;
using TestsPortal.BL.Services.Tests;
using TestsPortal.DAL.Repositories.ScheduledTests;
using ScheduledTestInstanceDAL = TestsPortal.DAL.Models.ScheduledTests.ScheduledTestInstance;
using ScheduledTestInstanceDomain = TestsPortal.Domain.ScheduledTests.ScheduledTestInstance;
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

        public IEnumerable<ScheduledTestInstanceDomain> ScheduleTest(ScheduledTestDomain scheduledTest)
        {
            var candidates = _candidateService.CreateCandidates(scheduledTest.Candidates);
            var test = _testsService.CreateTest(scheduledTest.Test);
            var dalScheduledTest = _mapper.Map<ScheduledTestDAL>(scheduledTest);
            dalScheduledTest.TestId = test.Id;
            _scheduledTestsRepository.Create(dalScheduledTest);
            var instances = MapTestInstances(dalScheduledTest.Id, scheduledTest, candidates).ToArray();
            _scheduledTestInstancesRepository.Create(instances);
            return _mapper.Map<IEnumerable<ScheduledTestInstanceDomain>>(instances);
        }

        public long GetInstanceOriginalId(long testId)
        {
            return _scheduledTestInstancesRepository.GetInstanceOriginalId(testId);
        }

        private IEnumerable<ScheduledTestInstanceDAL> MapTestInstances(
            long testId,
            ScheduledTestDomain test,
            IEnumerable<Domain.Candidates.Candidate> candidates)
        {
            return candidates.Select(x => new ScheduledTestInstanceDAL
            {
                CandidateId = x.Id,
                ScheduledTestId = testId,
                Status = (short)TestStatus.NotStarted,
                // TODO: fill these fields
                Keycode = "ABCDEF",
                Pin = "ABCDEF"
            });
        }
    }
}
