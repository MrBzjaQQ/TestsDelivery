using AutoMapper;
using TestsDelivery.DAL.Repositories.ScheduledTest;
using ScheduledTestDomain = TestsDelivery.Domain.ScheduledTest.ScheduledTest;
using ScheduledTestData = TestsDelivery.DAL.Models.ScheduledTest.ScheduledTest;
using System.Linq;
using System.Collections.Generic;
using TestsDelivery.BL.Services.Test;
using TestsDelivery.BL.Services.Candidates;
using TestsDelivery.DAL.Models.ScheduledTest;
using TestsDelivery.DAL.Repositories.CandidateInScheduledTest;
using TestsDelivery.Domain.ScheduledTest;
using TestsDelivery.Domain.Lists;
using TestsDelivery.BL.FilterBuilders.ScheduledTests;

namespace TestsDelivery.BL.Services.ScheduledTest
{
    public class ScheduledTestService : IScheduledTestService
    {
        private readonly IScheduledTestRepository _scheduledTestRepository;
        private readonly ITestService _testService;
        private readonly IMapper _mapper;
        private readonly IScheduledTestInstancesRepository _scheduledTestInstancesRepository;
        private readonly ICandidateService _candidateService;

        public ScheduledTestService(
            ITestService testService,
            IScheduledTestRepository scheduledTestRepository,
            ICandidateService candidateService,
            IScheduledTestInstancesRepository scheduledTestInstancesRepository,
            IMapper mapper)
        {
            _scheduledTestRepository = scheduledTestRepository;
            _candidateService = candidateService;
            _testService = testService;
            _mapper = mapper;
            _scheduledTestInstancesRepository = scheduledTestInstancesRepository;
        }

        public ScheduledTestDomain ScheduleTest(ScheduledTestDomain test)
        {
            var candidates = _candidateService.GetCandidates(test.Candidates.Select(x => x.Id));

            var testData = _mapper.Map<ScheduledTestData>(test);
            _scheduledTestRepository.Create(testData);

            _scheduledTestInstancesRepository.Create(MapTestInstances(testData.Id, candidates));
            var candidatesInScheduledTests = _scheduledTestInstancesRepository.GetByTestId(testData.Id);

            var testDomain = _mapper.Map<ScheduledTestDomain>(testData);
            testDomain.Test = _testService.GetFullTest(test.Test.Id);
            testDomain.Candidates = _mapper.Map<IEnumerable<Domain.Candidate.Candidate>>(candidatesInScheduledTests.Select(x => x.Candidate));

            return testDomain;
        }

        public ScheduledTestDomain GetTest(long id)
        {
            return _mapper.Map<ScheduledTestDomain>(_scheduledTestRepository.GetById(id));
        }

        public ScheduledTestsList GetList(ListFilter filter)
        {
            var filterBuilder = new ScheduledTestsFilterBuilder();

            if (filter.SearchText != null)
                filterBuilder.ByTestOrCandidateName(filter.SearchText);

            if (filter.Take.HasValue)
                filterBuilder.Take(filter.Take.Value);

            if (filter.Skip.HasValue)
                filterBuilder.Skip(filter.Skip.Value);

            var genericFilter = filterBuilder.Build();

            return new ScheduledTestsList
            {
                ScheduledTests = _mapper.Map<IEnumerable<ScheduledTestInListDto>>(_scheduledTestRepository.GetList(genericFilter)),
                TotalCount = _scheduledTestRepository.GetScheduledTestsCount(genericFilter)
            };
        }

        private IEnumerable<ScheduledTestInstance> MapTestInstances(
            long testId,
            IEnumerable<Domain.Candidate.Candidate> candidates)
        {
            return candidates.Select(x => new ScheduledTestInstance
            {
                CandidateId = x.Id,
                ScheduledTestId = testId,
                Status = (short)TestStatus.NotStarted,
                Keycode = "ABCDEF",
                Pin = "ABCDEF"
            });
        }
    }
}
