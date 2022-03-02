using AutoMapper;
using TestsDelivery.DAL.Repositories.Candidate;
using TestsDelivery.DAL.Repositories.ScheduledTest;
using ScheduledTestDomain = TestsDelivery.Domain.ScheduledTest.ScheduledTest;
using ScheduledTestData = TestsDelivery.DAL.Models.ScheduledTest.ScheduledTest;
using System.Linq;
using System.Collections.Generic;
using TestsDelivery.BL.Services.Test;
using TestsDelivery.BL.Services.Candidates;

namespace TestsDelivery.BL.Services.ScheduledTest
{
    public class ScheduledTestService : IScheduledTestService
    {
        private readonly IScheduledTestRepository _scheduledTestRepository;
        private readonly ITestService _testService;
        private readonly IMapper _mapper;
        private readonly ICandidateService _candidateService;

        public ScheduledTestService(
            ITestService testService,
            IScheduledTestRepository scheduledTestRepository,
            ICandidateService candidateService,
            IMapper mapper)
        {
            _scheduledTestRepository = scheduledTestRepository;
            _candidateService = candidateService;
            _testService = testService;
            _mapper = mapper;
        }

        public ScheduledTestDomain ScheduleTest(ScheduledTestDomain test)
        {
            var candidates = _candidateService.GetCandidates(test.Candidates.Select(x => x.Id));

            var testData = _mapper.Map<ScheduledTestData>(test);
            _scheduledTestRepository.ScheduleTest(testData);

            var testDomain = _mapper.Map<ScheduledTestDomain>(testData);
            testDomain.Candidates = _mapper.Map<IEnumerable<Domain.Candidate.Candidate>>(candidates);
            testDomain.Test = _testService.GetFullTest(test.Test.Id);

            return testDomain;
        }

        public ScheduledTestDomain GetTest(long id)
        {
            return _mapper.Map<ScheduledTestDomain>(_scheduledTestRepository.GetTest(id));
        }
    }
}
