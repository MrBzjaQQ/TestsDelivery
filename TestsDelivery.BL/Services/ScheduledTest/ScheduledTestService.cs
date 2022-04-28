using AutoMapper;
using TestsDelivery.DAL.Repositories.Candidate;
using TestsDelivery.DAL.Repositories.ScheduledTest;
using ScheduledTestDomain = TestsDelivery.Domain.ScheduledTest.ScheduledTest;
using ScheduledTestData = TestsDelivery.DAL.Models.ScheduledTest.ScheduledTest;
using System.Linq;
using System.Collections.Generic;
using TestsDelivery.BL.Services.Test;
using TestsDelivery.BL.Services.Candidates;
using TestsDelivery.DAL.Models.ScheduledTest;
using TestsDelivery.DAL.Repositories.CandidateInScheduledTest;

namespace TestsDelivery.BL.Services.ScheduledTest
{
    public class ScheduledTestService : IScheduledTestService
    {
        private readonly IScheduledTestRepository _scheduledTestRepository;
        private readonly ITestService _testService;
        private readonly IMapper _mapper;
        private readonly ICandidateInTestRepository _candidateInTestRepository;
        private readonly ICandidateService _candidateService;

        public ScheduledTestService(
            ITestService testService,
            IScheduledTestRepository scheduledTestRepository,
            ICandidateService candidateService,
            ICandidateInTestRepository candidateInTestRepository,
            IMapper mapper)
        {
            _scheduledTestRepository = scheduledTestRepository;
            _candidateService = candidateService;
            _testService = testService;
            _mapper = mapper;
            _candidateInTestRepository = candidateInTestRepository;
        }

        public ScheduledTestDomain ScheduleTest(ScheduledTestDomain test)
        {
            var candidates = _candidateService.GetCandidates(test.Candidates.Select(x => x.Id));

            var testData = _mapper.Map<ScheduledTestData>(test);
            _scheduledTestRepository.Create(testData);

            _candidateInTestRepository.Create(MapCandidatesInTests(testData.Id, candidates));
            var candidatesInScheduledTests = _candidateInTestRepository.GetByTestId(testData.Id);

            var testDomain = _mapper.Map<ScheduledTestDomain>(testData);
            testDomain.Test = _testService.GetFullTest(test.Test.Id);
            testDomain.Candidates = _mapper.Map<IEnumerable<Domain.Candidate.Candidate>>(candidatesInScheduledTests.Select(x => x.Candidate));

            return testDomain;
        }

        public ScheduledTestDomain GetTest(long id)
        {
            return _mapper.Map<ScheduledTestDomain>(_scheduledTestRepository.GetById(id));
        }

        private IEnumerable<CandidateInScheduledTest> MapCandidatesInTests(
            long testId,
            IEnumerable<Domain.Candidate.Candidate> candidates)
        {
            return candidates.Select(x => new CandidateInScheduledTest
            {
                CandidateId = x.Id,
                ScheduledTestId = testId
            });
        }
    }
}
