using TestsPortal.Domain.ScheduledTests;
using TestsPortal.BL.Services.Candidates;
using AutoMapper;
using TestsPortal.BL.Services.Tests;
using TestsPortal.DAL.Repositories.ScheduledTests;

namespace TestsPortal.BL.Services.ScheduledTests
{
    public class ScheduledTestService : IScheduledTestService
    {
        private readonly ICandidateService _candidateService;
        private readonly ITestsService _testsService;
        private readonly IScheduledTestsRepository _scheduledTestsRepository;
        private readonly IMapper _mapper;

        public ScheduledTestService(
            ICandidateService candidateService,
            ITestsService testsService,
            IScheduledTestsRepository scheduledTestsRepository,
            IMapper mapper)
        {
            _candidateService = candidateService;
            _testsService = testsService;
            _scheduledTestsRepository = scheduledTestsRepository;
            _mapper = mapper;
        }

        public ScheduledTest ScheduleTest(ScheduledTest scheduledTest)
        {
            var candidates = _candidateService.CreateCandidates(scheduledTest.Candidates);
            var test = _testsService.CreateTest(scheduledTest.Test);
            var dalScheduledTest = _mapper.Map<DAL.Models.ScheduledTests.ScheduledTest>(scheduledTest);

            _scheduledTestsRepository.CreateScheduledTest(dalScheduledTest);

            var resultTest = _mapper.Map<ScheduledTest>(dalScheduledTest);
            resultTest.Candidates = candidates;
            resultTest.Test = test;
            return resultTest;
        }
    }
}
