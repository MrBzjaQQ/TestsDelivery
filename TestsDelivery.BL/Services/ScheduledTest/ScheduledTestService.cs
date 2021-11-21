using AutoMapper;
using TestsDelivery.DAL.Repositories.Candidate;
using TestsDelivery.DAL.Repositories.ScheduledTest;
using TestsDelivery.Domain.Candidate;
using ScheduledTestDomain = TestsDelivery.Domain.ScheduledTest.ScheduledTest;
using ScheduledTestData = TestsDelivery.DAL.Models.ScheduledTest.ScheduledTest;

namespace TestsDelivery.BL.Services.ScheduledTest
{
    public class ScheduledTestService : IScheduledTestService
    {
        private readonly IScheduledTestRepository _scheduledTestRepository;
        private readonly IMapper _mapper;
        private readonly ICandidateRepository _candidateRepository;

        public ScheduledTestService(
            IScheduledTestRepository scheduledTestRepository,
            ICandidateRepository candidateRepository,
            IMapper mapper)
        {
            _scheduledTestRepository = scheduledTestRepository;
            _candidateRepository = candidateRepository;
            _mapper = mapper;
        }

        public ScheduledTestDomain ScheduleTest(ScheduledTestDomain test)
        {
            var candidate = _candidateRepository.GetCandidate(test.Candidate.Id);

            var testData = _mapper.Map<ScheduledTestData>(test);
            _scheduledTestRepository.ScheduleTest(testData);

            var testDomain = _mapper.Map<ScheduledTestDomain>(testData);
            testDomain.Candidate = _mapper.Map<Candidate>(candidate);

            return testDomain;
        }

        public ScheduledTestDomain GetTest(long id)
        {
            return _mapper.Map<ScheduledTestDomain>(_scheduledTestRepository.GetTest(id));
        }
    }
}
