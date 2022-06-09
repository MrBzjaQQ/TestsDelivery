using TestsPortal.BL.Services.Candidates;
using AutoMapper;
using TestsPortal.BL.Services.Tests;
using TestsPortal.DAL.Repositories.ScheduledTests;
using ScheduledTestInstanceDAL = TestsPortal.DAL.Models.ScheduledTests.ScheduledTestInstance;
using ScheduledTestInstanceDomain = TestsPortal.Domain.ScheduledTests.ScheduledTestInstance;
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

        public IEnumerable<ScheduledTestInstanceDomain> ScheduleTest(ScheduledTestToCreate scheduledTest)
        {
            var candidates = _candidateService.CreateCandidates(scheduledTest.Instances.Select(x => x.Candidate).ToArray());
            var test = _testsService.CreateTest(scheduledTest.Test);
            var dalScheduledTest = _mapper.Map<ScheduledTestDAL>(scheduledTest);
            dalScheduledTest.TestId = test.Id;
            _scheduledTestsRepository.Create(dalScheduledTest);
            var instances = _mapper.Map<IEnumerable<ScheduledTestInstanceDAL>>(scheduledTest.Instances);
            foreach (var instance in instances)
                instance.ScheduledTestId = dalScheduledTest.Id;
            _scheduledTestInstancesRepository.Create(instances);
            return _mapper.Map<IEnumerable<ScheduledTestInstanceDomain>>(instances);
        }

        public long GetInstanceOriginalId(long testId)
        {
            return _scheduledTestInstancesRepository.GetInstanceOriginalId(testId);
        }
    }
}
