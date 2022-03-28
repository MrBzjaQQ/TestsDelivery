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
        private readonly IMapper _mapper;

        public ScheduledTestService(
            ICandidateService candidateService,
            ITestsService testsService,
            IMapper mapper)
        {
            _candidateService = candidateService;
            _testsService = testsService;
            _mapper = mapper;
        }

        public ScheduledTest ScheduleTest(ScheduledTest scheduledTest)
        {
            var candidates = _candidateService.CreateCandidates(scheduledTest.Candidates);
            var test = _testsService.CreateTest(scheduledTest.Test);
            // TODO: schedule service
            throw new NotImplementedException();
        }
    }
}
