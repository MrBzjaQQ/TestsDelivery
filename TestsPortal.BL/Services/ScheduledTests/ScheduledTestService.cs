using TestsPortal.Domain.ScheduledTests;
using TestsPortal.BL.Services.Candidates;
using AutoMapper;
using TestsPortal.BL.Services.Tests;

namespace TestsPortal.BL.Services.ScheduledTests
{
    public class ScheduledTestService : IScheduledTestService
    {
        private readonly ICandidateService _candidateService;
        private readonly ITestsService _testsService;
        private readonly IScheduledTestService _scheduledTestRepository;
        private readonly IMapper _mapper;

        public ScheduledTestService(
            ICandidateService candidateService,
            ITestsService testsService,
            IScheduledTestService scheduledTestRepository,
            IMapper mapper)
        {
            _candidateService = candidateService;
            _testsService = testsService;
            _scheduledTestRepository = scheduledTestRepository;
            _mapper = mapper;
        }

        public ScheduledTest ScheduleTest(ScheduledTest scheduledTest)
        {
            var candidates = _candidateService.CreateCandidates(scheduledTest.Candidates);
            var test = _testsService.CreateTest(scheduledTest.Test);
            var dalScheduledTest = _mapper.Map<DAL.Models.ScheduledTests.ScheduledTest>(scheduledTest);
            _scheduledTestRepository.ScheduleTest(scheduledTest);
            // TODO: schedule service
            throw new NotImplementedException();
        }
    }
}
